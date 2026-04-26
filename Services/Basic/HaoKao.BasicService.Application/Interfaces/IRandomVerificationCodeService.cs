using HaoKao.BasicService.Application.ViewModels;

namespace HaoKao.BasicService.Application.Interfaces;

public interface IRandomVerificationCodeService : IAppWebApiService, IManager
{
    /// <summary>
    /// 获取随机码图片
    /// </summary>
    /// <param name="randomMark"></param>
    /// <returns></returns>
    Task<string> GetRandomVerificationCode(string randomMark);

    /// <summary>
    /// 获取随机码图片和较验Md5密文
    /// </summary>
    /// <param name="randomMark"></param>
    /// <returns></returns>
    Task<VerificationCodeViewModel> GetRandomVerification(string randomMark);

    /// <summary>
    /// 校验验证码
    /// </summary>
    /// <param name="randomMark"></param>
    /// <param name="code"></param>
    /// <returns></returns>
    Task<bool> VerificationCode(string randomMark, string code);

    string GetRandomNumber(int minValue, int maxValue);
}