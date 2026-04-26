using HaoKao.CourseService.Application.Modules.CourseVideoModule.Interfaces;
using HaoKao.CourseService.Application.Modules.CourseVideoModule.ViewModels;

namespace HaoKao.CourseService.Application.Modules.CourseVideoModule.Services;

/// <summary>
/// 课程视频接口服务
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.WeChat)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class CourseVideoWeChatService(ICourseVideoWebService service) : ICourseVideoWeChatService
{
    private readonly ICourseVideoWebService _service = service ?? throw new ArgumentNullException(nameof(service));

    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    [HttpGet("{id}")]
    public Task<BrowseCourseVideoViewModel> Get(Guid id)
    {
        return _service.Get(id);
    }

    /// <summary>
    /// 根据查询获取列表，用于分页(读取课程下面所有的课程视频id)
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    [HttpGet]
    [AllowAnonymous]
    public Task<QueryCourseVideoViewModel> GetCourseVideoList([FromQuery] QueryCourseVideoViewModel queryViewModel)
    {
        return _service.GetCourseVideoList(queryViewModel);
    }

    /// <summary>
    /// 读取课程下面的更新资料接口
    /// </summary>
    /// <param name="CourseIds"></param>
    /// <returns></returns>
    [HttpGet]
    public Task<List<UpdateCourseVideoNewViewModel>> GetUpdateCourseVideoList([FromQuery] string CourseIds)
    {
        return _service.GetUpdateCourseVideoList(CourseIds);
    }

    /// <summary>
    /// 读取课程下面的更新资料接口(智辅课程专用)
    /// </summary>
    /// <param name="courseIds"></param>
    /// <returns></returns>
    [HttpGet]
    public Task<List<UpdateCourseVideoNewViewModel>> GetUpdateAssistantCourseVideoList([FromQuery] string courseIds)
    {
        return _service.GetUpdateAssistantCourseVideoList(courseIds);
    }

    /// <summary>
    /// 根据课程ids拿到多个课程下面所有的
    /// </summary>
    /// <param name="CourseIds"></param>
    /// <returns></returns>
    [HttpGet]
    public Task<List<CourseVideoQueryListViewModel>> GetVideoIdsByCourseIds([FromQuery] string CourseIds)
    {
        return _service.GetVideoIdsByCourseIds(CourseIds);
    }

    /// <summary>
    /// 根据章节id拉取章节下面的视频
    /// </summary>
    /// <param name="queryViewModel"></param>
    /// <returns></returns>
    public Task<QueryCourseVideoViewModel> Get([FromQuery] QueryCourseVideoViewModel queryViewModel)
    {
        return _service.Get(queryViewModel);
    }

    /// <summary>
    /// 读取视频详情(批量拉取)
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost]
    public Task<List<BrowseCourseVideoViewModel>> GetVideoInfo([FromBody] QueryByIds model)
    {
        return _service.GetVideoInfo(model);
    }

    /// <summary>
    /// 通过知识点Id获取关联知识点对应的学习视频
    /// </summary>
    /// <param name="CourseChapterId">课程id</param>
    /// <param name="knowledgePointIds">知识点id数组</param>
    /// <returns></returns>
    [HttpPost]
    public Task<List<BrowseCourseVideoViewModel>> GetVideoInfoByKnowledgePointId(Guid? CourseChapterId, [FromBody] List<Guid> knowledgePointIds)
    {
        return service.GetVideoInfoByKnowledgePointId(CourseChapterId, knowledgePointIds);
    }
}