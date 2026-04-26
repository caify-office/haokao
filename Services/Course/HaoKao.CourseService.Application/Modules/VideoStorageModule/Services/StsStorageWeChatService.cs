using HaoKao.CourseService.Application.Modules.VideoStorageModule.Interfaces;

namespace HaoKao.CourseService.Application.Modules.VideoStorageModule.Services;

/// <summary>
/// 获取云存储临时访问凭证STS相关接口 - 微信小程序
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.WeChat)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class StsStorageWeChatService(IVideoStorageService service) : IStsStorageWeChatService
{
    private readonly IVideoStorageService _service = service ?? throw new ArgumentNullException(nameof(service));

    /// <summary>
    /// 获取STS凭证
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<dynamic> AssumeRole()
    {
        var handler = await _service.GetVideoStorageHandler();
        return handler.AssumeRole();
    }
}