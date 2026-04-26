using ShortUrlService.Domain.Commands;
using ShortUrlService.Domain.Entities;
using ShortUrlService.Domain.Repositories;
using ShortUrlService.WebApi.Configurations;
using ShortUrlService.WebApi.Models;
using ShortUrlService.WebApi.Proxies;
using ShortUrlService.WebApi.Shorteners;

namespace ShortUrlService.WebApi.Services;

public interface IShortUrlService : IAppWebApiService, IManager
{
    /// <summary>
    /// 访问短链
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<IActionResult> AccessAsync(AccessRequest request);

    /// <summary>
    /// 生成短链
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<GenerateResponse> GenerateAsync(GenerateRequest request);

    /// <summary>
    /// 默认过期时间生成端链时起：1年，访问次数：100000
    /// </summary>
    /// <param name="requestSimple"></param>
    /// <returns></returns>
    Task<Dictionary<string, GenerateResponse>> GenerateSimpleAsync(GenerateRequestSimple requestSimple);
}

[DynamicWebApi]
[Route("apiShortUrl/ShortUrlService")]
public class ShortUrlService(
    IMediatorHandler bus,
    IAccessLogQueue queue,
    IShortUrlRepository repository,
    IRegisterAppRepository registerAppRepo,
    IRedisConnectionWrapper connectionWrapper,
    IConfiguration configuration,
    IOptionsSnapshot<ShortUrlConfig> options,
    Base62Shortener shortener,
    IdWorker idWorker,
    IMapper mapper
) : IShortUrlService
{
    #region 私有成员

    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly IAccessLogQueue _queue = queue ?? throw new ArgumentNullException(nameof(queue));
    private readonly IShortUrlRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly IRegisterAppRepository _registerAppRepo = registerAppRepo ?? throw new ArgumentNullException(nameof(registerAppRepo));
    private readonly RedisRetryProxy _redisProxy = new(connectionWrapper.GetDatabase((int)RedisDatabaseNumber.Cache), configuration);
    private readonly IOptionsSnapshot<ShortUrlConfig> _options = options ?? throw new ArgumentNullException(nameof(options));

    #endregion

    #region 访问短链

    [NonAction]
    public async Task<IActionResult> AccessAsync(AccessRequest request)
    {
        var id = shortener.Restore(request.ShortKey);
        var shortUrl = await GetShortUrlById(id);
        var expiredTime = shortUrl.ExpiredTime;

        // 检查是否过期
        if (expiredTime < DateTime.Now)
        {
            await _bus.SendCommand(new DeleteShortUrlCommand(id));
            return new ObjectResult("短链接已过期")
            {
                StatusCode = StatusCodes.Status403Forbidden,
            };
        }

        var accessLimit = shortUrl.AccessLimit;
        var accessCount = await GetAccessCount(id);

        // 检查次数是否用完
        if (accessLimit <= accessCount)
        {
            await _bus.SendCommand(new DeleteShortUrlCommand(id));
            return new ObjectResult("短链接访问次数已用完")
            {
                StatusCode = StatusCodes.Status403Forbidden,
            };
        }

        // 使用的 redis 自增 accessCount
        var incrementTask = IncrementAccessCount(id);

        // 更新可访问记录, 通过消息队列异步更新
        var httpContext = EngineContext.Current.HttpContext;
        var enqueueTask = _queue.EnqueueAsync(
            new AccessLog(
                idWorker.NextId(),
                id,
                httpContext.GetIpAddress(),
                (int)httpContext.GetSystemType(),
                GetBrowserType(httpContext),
                httpContext.Request.Headers.UserAgent.ToString()
            )
        ).AsTask();

        await Task.WhenAll(incrementTask, enqueueTask);

        return _options.Value.TestAccess ? new OkResult() : new RedirectResult(shortUrl.OriginUrl, false);
    }

    private async Task<int> GetAccessCount(long id)
    {
        var countStr = await _redisProxy.StringGetAsync($"{nameof(ShortUrl)}:{id}:AccessCount");
        if (countStr.HasValue)
        {
            return (int)countStr;
        }
        var shortUrl = await GetShortUrlById(id);
        var accessCount = await _repository.GetAccessCountAsync(shortUrl.Id);
        await _redisProxy.StringSetAsync($"{nameof(ShortUrl)}:{id}:AccessCount", accessCount, TimeSpan.FromMinutes(60));
        return accessCount;
    }

    private Task<long> IncrementAccessCount(long id)
    {
        return _redisProxy.StringIncrementAsync($"{nameof(ShortUrl)}:{id}:AccessCount");
    }

    private async Task<ShortUrl> GetShortUrlById(long id)
    {
        var json = await _redisProxy.StringGetAsync($"{nameof(ShortUrl)}:{id}");
        if (json.HasValue)
        {
            return JsonConvert.DeserializeObject<ShortUrl>(json)
                ?? throw new NullReferenceException("ShortUrl dose not exist!");
        }

        var shortUrl = await _repository.GetByIdAsync(id)
                    ?? throw new NullReferenceException("ShortUrl dose not exist!");

        var cacheTime = TimeSpan.FromSeconds((shortUrl.ExpiredTime - DateTime.Now).TotalSeconds);
        await _redisProxy.StringSetAsync($"{nameof(ShortUrl)}:{id}", JsonConvert.SerializeObject(shortUrl), cacheTime);
        return shortUrl;
    }

    public static int GetBrowserType(HttpContext context)
    {
        return context.Request.Headers.UserAgent.ToString() switch
        {
            "MSIE" => (int)BrowserType.IE,
            "QQBrowser" => (int)BrowserType.QQ,
            "MicroMessenger" => (int)BrowserType.WeChat,
            "Firefox" => (int)BrowserType.Firefox,
            "Microsoft Edge" => 7,
            "Chrome" => (int)BrowserType.Chrome,
            "Safari" => (int)BrowserType.Safari,
            _ => (int)BrowserType.Default
        };
    }

    #endregion

    #region 生成短链

    [HttpPost("Generate")]
    public async Task<GenerateResponse> GenerateAsync([FromBody] GenerateRequest request)
    {
        // 校验应用密钥
        var registerApp = await _registerAppRepo.GetByCodeAndSecret(request.AppCode, request.AppSecret)
                       ?? throw new GirvsException(StatusCodes.Status404NotFound, "注册应用不存在");

        // 校验域名
        if (!registerApp.AppDomains.Any(domain => IsUrlBelongsToDomain(request.OriginUrl, domain)))
        {
            throw new GirvsException("The domain is not allowed to generate short links.");
        }

        var shortUrl = await repository.GetForRegisterApp(registerApp.Id, request.OriginUrl);

        Uri url;
        string qrCodeBase64;

        if (shortUrl != null)
        {
            url = GetFullUrl(shortUrl.ShortKey);
            qrCodeBase64 = GenerateBase64QrCode(url);
            return new GenerateResponse(shortUrl.ShortKey, url, qrCodeBase64);
        }

        var createCommand = new CreateShortUrlCommand(registerApp.Id, request.OriginUrl, "", request.AccessLimit, request.ExpiredTime);
        shortUrl = (ShortUrl)await _bus.SendCommand(createCommand);

        var shortKey = shortener.Short(shortUrl.Id);
        var updateCommand = new UpdateShortKeyCommand(shortUrl.Id, shortKey);
        await _bus.SendCommand(updateCommand);

        await SetShortUrlCache(shortUrl);

        url = GetFullUrl(shortKey);
        qrCodeBase64 = GenerateBase64QrCode(url);
        return new GenerateResponse(shortKey, url, qrCodeBase64);
    }

    /// <summary>
    /// 默认过期时间生成端链时起：3年，访问次数：100000
    /// </summary>
    /// <param name="requestSimple"></param>
    /// <returns></returns>
    [HttpPost("GenerateSimple")]
    public Task<Dictionary<string, GenerateResponse>> GenerateSimpleAsync([FromBody] GenerateRequestSimple requestSimple)
    {
        var result = new Dictionary<string, GenerateResponse>();
        requestSimple.OriginUrlArray.ToList().ForEach(originUrl =>
        {
            if (!result.ContainsKey(originUrl))
            {
                var request = new GenerateRequest(
                    originUrl
                  , 100000
                  , DateTime.Now.AddYears(3)
                  , requestSimple.AppCode
                  , requestSimple.AppSecret
                );
                var shorUrl = GenerateAsync(request).Result;
                result.Add(originUrl, shorUrl);
            }
        });

        return Task.FromResult(result);
    }

    private static bool IsUrlBelongsToDomain(string url, string domain)
    {
        var uri = new Uri(url);
        return uri.Host.EndsWith(domain, StringComparison.OrdinalIgnoreCase);
    }

    private Uri GetFullUrl(string shortKey)
    {
        return new Uri(_options.Value.HostDomain, shortKey);
    }

    private static string GenerateBase64QrCode(Uri url)
    {
        var plantText = url.ToString();
        using var qrGenerator = new QRCodeGenerator();
        using var qrCodeData = qrGenerator.CreateQrCode(plantText, QRCodeGenerator.ECCLevel.Q);
        using var qrCode = new Base64QRCode(qrCodeData);
        return qrCode.GetGraphic(20);
    }

    private Task<bool[]> SetShortUrlCache(ShortUrl shortUrl)
    {
        var cacheTime = TimeSpan.FromSeconds((shortUrl.ExpiredTime - DateTime.Now).TotalSeconds);
        return Task.WhenAll([
            _redisProxy.StringSetAsync($"{nameof(ShortUrl)}:{shortUrl.Id}", JsonConvert.SerializeObject(shortUrl), cacheTime),
            _redisProxy.StringSetAsync($"{nameof(ShortUrl)}:{shortUrl.Id}:AccessCount", 0, cacheTime)
        ]);
    }

    #endregion

    #region 管理端

    /// <summary>
    /// 根据Id获取短链接
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id:long}")]
    public async Task<ShortUrlDto> Get(long id)
    {
        var entity = await repository.GetByIdAsync(id);
        return mapper.Map<ShortUrlDto>(entity);
    }

    /// <summary>
    /// 获取短链接列表
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<PagingResult<ShortUrlPagingResult>> Get([FromQuery] ShortUrlPagingRequest request)
    {
        var (totalCount, data) = await repository.GetPagedListAsync(request.RegisterAppId, request.PageIndex, request.PageSize);
        var dict = await _repository.GetAccessCountAsync(data.Select(x => x.Id));
        return new PagingResult<ShortUrlPagingResult>(totalCount, data.Select(x => new ShortUrlPagingResult
        {
            Id = x.Id,
            ShortKey = x.ShortKey,
            OriginUrl = x.OriginUrl,
            AccessLimit = x.AccessLimit,
            ExpiredTime = x.ExpiredTime,
            CreateTime = x.CreateTime,
            ConsumedCount = dict.GetValueOrDefault(x.Id),
            FullUrl = GetFullUrl(x.ShortKey).ToString()
        }).ToList());
    }

    /// <summary>
    /// 创建短链接
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<GenerateResponse> Create([FromBody] CreateShortUrlRequest request)
    {
        var registerApp = await _registerAppRepo.GetByIdAsync(request.RegisterAppId)
                       ?? throw new GirvsException(StatusCodes.Status404NotFound, "注册应用不存在");
        return await GenerateAsync(
            new GenerateRequest(
                request.OriginUrl,
                request.AccessLimit,
                request.ExpiredTime,
                registerApp.AppCode,
                registerApp.AppSecret));
    }

    /// <summary>
    /// 更新短链接
    /// </summary>
    /// <param name="request"></param>
    [HttpPut]
    public async Task Update([FromBody] UpdateShortUrlRequest request)
    {
        var updateCommand = mapper.Map<UpdateShortUrlCommand>(request);
        await _bus.SendCommand(updateCommand);
    }

    #endregion
}