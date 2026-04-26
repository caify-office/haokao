using Girvs.Extensions;
using HaoKao.Common;
using HaoKao.OpenPlatformService.Application.ViewModels;
using Microsoft.Extensions.Logging;

namespace HaoKao.OpenPlatformService.Application.Services.Web;

[DynamicWebApi(Module = ServiceAreaName.WebSite)]
public class RandomVerificationCodeService(
    IStaticCacheManager staticCacheManager,
    ILogger<RandomVerificationCodeService> logger) : IRandomVerificationCodeService
{
    private readonly ILogger<RandomVerificationCodeService> _logger = logger;
    private readonly string _randomVerificationCodeTemplate = "RegisteredUserServiceService:RandomVerificationCode:{0}";

    private CacheKey GetCacheKey(string randomMark)
    {
        var cacheKey = new CacheKey(_randomVerificationCodeTemplate).Create(randomMark.ToMd5());
        cacheKey.CacheTime = 3;
        return cacheKey;
    }

    /// <summary>
    /// 获取随机码图片
    /// </summary>
    /// <param name="randomMark"></param>
    /// <returns></returns>
    [HttpGet("{randomMark}")]
    public async Task<string> GetRandomVerificationCode(string randomMark)
    {
        var model = await GetRandomVerification(randomMark);
        return model.VerificationCodeImageBase64;
    }

    /// <summary>
    /// 获取随机码图片和较验Md5密文
    /// </summary>
    /// <param name="randomMark"></param>
    /// <returns></returns>
    [HttpGet("{randomMark}")]
    public async Task<VerificationCodeViewModel> GetRandomVerification(string randomMark)
    {
        var cacheKey = GetCacheKey(randomMark);
        #pragma warning disable CS0618
        var g = new SecureRandomNumberGenerator();
        #pragma warning restore CS0618
        var r = g.Next(100000, 999999).ToString();
        await staticCacheManager.SetAsync(cacheKey, r);
        var result = ValidateCodeHelper.CreateBase64ImageSrc(r);

        return new VerificationCodeViewModel
        {
            VerificationCodeImageBase64 = result,
            VerificationCodeMd5 = r.ToMd5()
        };
    }

    [NonAction]
    public async Task<bool> VerificationCode(string randomMark, string code)
    {
        var cacheKey = GetCacheKey(randomMark);
        var cacheValue = staticCacheManager.Get(cacheKey, () => string.Empty);
        if (cacheValue.IsNullOrEmpty() || cacheValue != code)
        {
           await staticCacheManager.RemoveAsync(cacheKey);
            throw new GirvsException("验证码校验错误，或验证码失效已过期");
        }

        return true;
    }
}