using HaoKao.CourseRatingService.Application.Services.Web;
using HaoKao.CourseRatingService.Application.ViewModels;

namespace HaoKao.CourseRatingService.Application.Services.WeChat;

/// <summary>
/// 课程评价接口服务微信端
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.WeChat)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class CourseRatingWeChatService(ICourseRatingWebService service) : ICourseRatingWeChatService
{
    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    [HttpGet, AllowAnonymous]
    public Task<QueryCourseRatingWebViewModel> Get([FromQuery] QueryCourseRatingWebViewModel queryViewModel)
    {
        return service.Get(queryViewModel);
    }

    /// <summary>
    /// 创建课程评价
    /// </summary>
    /// <param name="model">新增模型</param>
    [HttpPost]
    public Task Create([FromBody] CreateCourseRatingViewModel model)
    {
        return service.Create(model);
    }
}