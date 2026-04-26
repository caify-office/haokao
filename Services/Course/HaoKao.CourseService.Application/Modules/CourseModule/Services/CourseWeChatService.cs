using HaoKao.CourseService.Application.Modules.CourseModule.Interfaces;
using HaoKao.CourseService.Application.Modules.CourseModule.ViewModels;

namespace HaoKao.CourseService.Application.Modules.CourseModule.Services;

/// <summary>
/// 课程服务
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.WeChat)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class CourseWeChatService(ICourseWebService service) : ICourseWeChatService
{
    private readonly ICourseWebService _service = service ?? throw new ArgumentNullException(nameof(service));

    /// <summary>
    /// 根据查询获取列表
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    [HttpGet]
    public async Task<QueryCourseViewModel> Get([FromQuery] QueryCourseViewModel queryViewModel)
    {
        return await _service.Get(queryViewModel);
    }

    /// <summary>
    /// 读取课程详情
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    [AllowAnonymous]
    public async Task<BrowseCourseViewModel> GetCourseInfo(Guid id)
    {
        return await _service.GetCourseInfo(id);
    }

    /// <summary>
    /// 批量读取课程详情
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [HttpGet]
    [AllowAnonymous]
    public async Task<List<BrowseCourseViewModel>> GetCourseInfos(string ids)
    {
        return await _service.GetCourseInfos(ids);
    }

    /// <summary>
    /// 根据课程ids集合拿到课程下面的所有的讲师ids(去重)
    /// </summary>
    /// <param name="queryViewModel"></param>
    /// <returns></returns>
    [HttpGet]
    [AllowAnonymous]
    public async Task<List<string>> GetTeacherIds([FromQuery] QueryCourseViewModel queryViewModel)
    {
        return await _service.GetTeacherIds(queryViewModel);
    }
}