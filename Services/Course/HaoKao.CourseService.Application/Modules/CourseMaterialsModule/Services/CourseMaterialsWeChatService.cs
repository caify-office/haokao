using HaoKao.CourseService.Application.Modules.CourseMaterialsModule.Interfaces;
using HaoKao.CourseService.Application.Modules.CourseMaterialsModule.ViewModels;
using HaoKao.CourseService.Domain.CourseMaterialsModule;

namespace HaoKao.CourseService.Application.Modules.CourseMaterialsModule.Services;

/// <summary>
/// 课程讲义接口服务
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.WeChat)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class CourseMaterialsWeChatService(ICourseMaterialsWebService service) : ICourseMaterialsWeChatService
{
    private readonly ICourseMaterialsWebService _service = service ?? throw new ArgumentNullException(nameof(service));

    /// <summary>
    /// 根据查询获取列表，用于分页(读取课程下所有的讲义)
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    [HttpGet]
    public Task<QueryCourseMaterialsViewModel> Get([FromQuery] QueryCourseMaterialsViewModel queryViewModel)
    {
        return _service.Get(queryViewModel);
    }

    /// <summary>
    ///下载讲义（阶段学习传课程章节Id，智辅课程学习传课程Id）
    /// </summary>
    /// <param name="CourseId">（阶段学习传课程章节Id，智辅课程学习传课程Id）</param>
    /// <returns></returns>
    [HttpGet]
    public Task<List<CourseMaterials>> DownLoadMaterials([FromQuery] Guid CourseId)
    {
        return _service.DownLoadMaterials(CourseId);
    }
}