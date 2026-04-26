using HaoKao.CourseService.Application.Modules.CourseModule.Interfaces;
using HaoKao.CourseService.Application.Modules.CourseModule.ViewModels;
using HaoKao.CourseService.Domain.CourseModule;
using Newtonsoft.Json;

namespace HaoKao.CourseService.Application.Modules.CourseModule.Services;

/// <summary>
/// 课程服务
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.WebSite)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class CourseWebService(ICourseService service, ICourseRepository repository) : ICourseWebService
{
    /// <summary>
    /// 根据查询获取列表
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    [HttpGet]
    public Task<QueryCourseViewModel> Get([FromQuery] QueryCourseViewModel queryViewModel)
    {
        return service.Get(queryViewModel);
    }

    /// <summary>
    /// 读取课程详情
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    [AllowAnonymous]
    public Task<BrowseCourseViewModel> GetCourseInfo(Guid id)
    {
        return service.Get(id);
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
        var courses = await repository.GetWhereAsync(x => ids.Contains(x.Id.ToString()));
        return courses.MapTo<List<BrowseCourseViewModel>>();
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
        queryViewModel = await service.Get(queryViewModel);
        return queryViewModel.Result?.Select(x => JsonConvert.DeserializeObject<List<TeacherJsonViewModel>>(x.TeacherJson))
                             .SelectMany(x => x).Select(x => x.Id)
                             .Distinct().ToList();
    }
}