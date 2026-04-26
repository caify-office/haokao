using HaoKao.CourseFeatureService.Application.Interfaces;
using HaoKao.CourseFeatureService.Application.ViewModels;

namespace HaoKao.CourseFeatureService.Application.Services;

/// <summary>
/// 课程特色服务接口服务
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.WeChat)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class CourseFeatureWeChatService(ICourseFeatureWebService appService) : ICourseFeatureWeChatService
{
    private readonly ICourseFeatureWebService _appService = appService ?? throw new ArgumentNullException(nameof(appService));

    /// <summary>
    /// 根据ids获取列表
    /// </summary>
    /// <param name="ids"></param>
    [HttpPost, AllowAnonymous]
    public Task<List<BrowseCourseFeatureWebViewModel>> Get([FromBody] List<Guid> ids)
    {
        return _appService.Get(ids);
    }
}