using HaoKao.AuditLogService.Domain.Entities;
using HaoKao.AuditLogService.Domain.Queries;
using HaoKao.AuditLogService.Domain.Repositories;
using HaoKao.Common;
using HaoKao.Common.Services;

namespace HaoKao.AuditLogService.Application.Services.Management;

[DynamicWebApi(Module = ServiceAreaName.Management)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsJwt)]
[ServicePermissionDescriptor(
    "审计日志管理",
    "a909abc7-696a-4a38-badf-cc5a2065220f",
    "32",
    SystemModule.All,
    3
)]
public class AuditLogService(
    IAuditLogRepository repository,
    IStaticCacheManager staticCacheManager,
    IRedisCacheService redisCacheService,
    ILocker locker
) : IAuditLogService
{
    private readonly IAuditLogRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly IStaticCacheManager _staticCacheManager = staticCacheManager ?? throw new ArgumentNullException(nameof(staticCacheManager));
    private readonly IRedisCacheService _redisCacheService = redisCacheService ?? throw new ArgumentNullException(nameof(redisCacheService));
    private readonly ILocker _locker = locker ?? throw new ArgumentNullException(nameof(locker));

    /// <summary>
    /// 根据Id获取对应的操作日志详细信息
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id:guid}")]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View)]
    public async Task<DetailAuditLogViewModel> Get(Guid id)
    {
        var log = await _staticCacheManager.GetAsync(
            GirvsEntityCacheDefaults<AuditLog>.ByIdCacheKey.Create(id.ToString()),
            () => _repository.GetByIdAsync(id)
        ) ?? throw new GirvsException("未找到对应的日志消息", StatusCodes.Status404NotFound);

        return log.MapToDto<DetailAuditLogViewModel>();
    }

    /// <summary>
    /// 根据查询列表获取日志列表
    /// </summary>
    /// <param name="queryViewModel"></param>
    /// <returns></returns>
    [HttpGet]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View)]
    public async Task<AuditLogQueryViewModel> Get(AuditLogQueryViewModel queryViewModel)
    {
        var (queryFields, _) = typeof(ListAuditLogViewModel).GetTypeQueryFieldsAndCacheKey();

        var query = queryViewModel.MapToQuery<AuditLogQuery>();
        query.QueryFields = queryFields;
        query.OrderBy = nameof(AuditLog.CreateTime);

        await _repository.GetByQueryAsync(query);

        return query.MapToQueryDto<AuditLogQueryViewModel, AuditLog>();
    }

    [HttpGet("RemoteIpAddress")]
    [AllowAnonymous]
    public string GetRemoteIpAddress()
    {
        return EngineContext.Current.HttpContext.Request.GetUserRemoteIpAddress();
    }

    [HttpGet("GirvsRemoteIpAddress")]
    [AllowAnonymous]
    public Task<string> GetGirvsRemoteIpAddress()
    {
        return Task.FromResult(EngineContext.Current.HttpContext.Request.GetApiGateWayRemoteIpAddress());
    }

    [HttpGet("ClientIpList")]
    [AllowAnonymous]
    public IEnumerable<string> GetClientIpList()
    {
        var remoteIp = EngineContext.Current.HttpContext.Connection.RemoteIpAddress;
        var ip = remoteIp.MapToIPv4().ToString();
        var scheme = EngineContext.Current.HttpContext.Request.Scheme;
        var host = EngineContext.Current.HttpContext.Request.Host;
        var ho = host.ToString();
        return [ip, scheme, ho,];
    }

    [HttpGet("RemoteIpList")]
    [AllowAnonymous]
    public IEnumerable<string> GetRemoteIpList()
    {
        var remoteIp = EngineContext.Current.HttpContext.Connection.RemoteIpAddress;
        var ip = remoteIp.MapToIPv4().ToString();
        var scheme = EngineContext.Current.HttpContext.Request.Scheme;
        var host = EngineContext.Current.HttpContext.Request.Host;
        var ho = host.ToString();
        return [ip, scheme, ho,];
    }

    [HttpGet("NginxHeaderIpList")]
    [AllowAnonymous]
    public IEnumerable<string> GetHeaderIpList()
    {
        return EngineContext.Current.HttpContext.Request.Headers["X-Forwarded-For"];
    }

    [HttpGet("ApiGateWayIpList")]
    [AllowAnonymous]
    public string GetApiGateWayIpList()
    {
        _staticCacheManager.SetAsync(new CacheKey("testtest").Create("kicck"), "asdf");
        _locker.PerformActionWithLockAsync("testtesttest", TimeSpan.FromMinutes(30), () =>
        {
            _redisCacheService.RedisStringIncrementAsync("kicckTest");
            return Task.CompletedTask;
        }).Wait();

        return EngineContext.Current.HttpContext.Request.GetApiGateWayRemoteIpAddress();
    }
}