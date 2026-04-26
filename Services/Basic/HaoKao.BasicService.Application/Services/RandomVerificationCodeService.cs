using HaoKao.BasicService.Application.Interfaces;
using HaoKao.BasicService.Application.ViewModels;
using HaoKao.Common;
using System.Security.Cryptography;

namespace HaoKao.BasicService.Application.Services;

[DynamicWebApi(Module = ServiceAreaName.Management)]
public class RandomVerificationCodeService(IStaticCacheManager staticCacheManager) : IRandomVerificationCodeService
{
    private const string _RandomVerificationCodeTemplate = "BasicServiceUserServiceService:RandomVerificationCode:{0}";

    private static CacheKey GetCacheKey(string randomMark)
    {
        var cacheKey = new CacheKey(_RandomVerificationCodeTemplate).Create(randomMark.ToMd5());
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
        var next = GetRandomNumber(100000, 999999);
        var cacheKey = GetCacheKey(randomMark);
        await staticCacheManager.SetAsync(cacheKey, next);
        var result = ValidateCodeHelper.CreateBase64ImageSrc(next);

        return new VerificationCodeViewModel
        {
            VerificationCodeImageBase64 = result,
            VerificationCodeMd5 = next.ToMd5()
        };
    }

    /// <summary>
    /// 校验验证码
    /// </summary>
    /// <param name="randomMark"></param>
    /// <param name="code"></param>
    /// <returns></returns>
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

    [NonAction]
    public string GetRandomNumber(int minValue, int maxValue)
    {
        var data = RandomNumberGenerator.GetBytes(4);
        var nextDouble = BitConverter.ToUInt32(data, 0) / 4294967296.0;
        return $"{(int)Math.Floor(minValue + (maxValue - (double)minValue) * nextDouble)}";
    }
}