using HaoKao.CourseService.Application.Modules.CourseVideoModule.Interfaces;
using HaoKao.CourseService.Application.Modules.CourseVideoModule.ViewModels;
using HaoKao.CourseService.Domain.CourseChapterModule;
using HaoKao.CourseService.Domain.CourseVideoModule;
using System.Linq.Expressions;

namespace HaoKao.CourseService.Application.Modules.CourseVideoModule.Services;

/// <summary>
/// 课程视频接口服务
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.WebSite)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class CourseVideoWebService(
    ICourseVideoService service,
    ICourseVideoRepository repository,
    ICourseChapterRepository courseChapterRepository
) : ICourseVideoWebService
{
    #region 初始参数

    private readonly ICourseVideoService _service = service ?? throw new ArgumentNullException(nameof(service));
    private readonly ICourseVideoRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly ICourseChapterRepository _courseChapterRepository = courseChapterRepository ?? throw new ArgumentNullException(nameof(courseChapterRepository));

    #endregion

    #region 服务方法

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
    public async Task<QueryCourseVideoViewModel> GetCourseVideoList([FromQuery] QueryCourseVideoViewModel queryViewModel)
    {
        var chapters = _courseChapterRepository.GetWhereAsync(predicate => predicate.CourseId == queryViewModel.CourseId).Result;
        var query = queryViewModel.MapToQuery<CourseVideoQuery>();
        query.CourseChapterIds = chapters.Select(x => x.Id).ToList();
        await _repository.GetByQueryAsync(query);
        var result = query.MapToQueryDto<QueryCourseVideoViewModel, CourseVideo>();
        foreach (var item in result.Result)
        {
            var courseChapter = await _courseChapterRepository.GetAsync(p => p.Id == item.CourseChapterId);
            item.CourseChapterSort = courseChapter.Sort;
            item.CourseChapterName = courseChapter.Name;
            var daysSpan = new TimeSpan(DateTime.Now.Ticks - item.CreateTime.Ticks);
            item.IsNew = !(daysSpan.TotalDays > 3);
        }
        //这里不是按课程视频的sort来排序而是按章节那边的sort来排序,在课程章节表上增加sort排序字段,前端对应需要增加一个排序的功能
        result.Result = result.Result.OrderBy(x => x.CourseChapterSort).ToList();
        return result;
    }

    /// <summary>
    /// 读取课程下面的更新资料接口
    /// </summary>
    /// <param name="courseIds"></param>
    /// <returns></returns>
    [HttpGet]
    public Task<List<UpdateCourseVideoNewViewModel>> GetUpdateCourseVideoList([FromQuery] string courseIds)
    {
        return _service.GetUpdateCourseVideoList(courseIds);
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
    /// 根据课程ids拿到多个课程下面所有的课程视频信息
    /// </summary>
    /// <param name="courseIds"></param>
    /// <returns></returns>
    [HttpGet]
    public Task<List<CourseVideoQueryListViewModel>> GetVideoIdsByCourseIds([FromQuery] string courseIds)
    {
        return _service.GetVideoIdsByCourseIds(courseIds);
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
    public async Task<List<BrowseCourseVideoViewModel>> GetVideoInfo([FromBody] QueryByIds model)
    {
        var list = await _repository.GetWhereAsync(x => model.Ids.Contains(x.Id.ToString()));
        return list.MapTo<List<BrowseCourseVideoViewModel>>();
    }

    /// <summary>
    /// 通过知识点Id获取关联知识点对应的学习视频
    /// </summary>
    /// <param name="CourseChapterId"></param>
    /// <param name="knowledgePointIds">知识点id数组</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<List<BrowseCourseVideoViewModel>> GetVideoInfoByKnowledgePointId(Guid? CourseChapterId, [FromBody] List<Guid> knowledgePointIds)
    {
        Expression<Func<CourseVideo, bool>> expression = x => knowledgePointIds.Contains(x.KnowledgePointId);
        if (CourseChapterId.HasValue)
        {
            expression = expression.And(x => x.CourseChapterId == CourseChapterId);
        }
        var list = await _repository.GetWhereAsync(expression);
        return list.MapTo<List<BrowseCourseVideoViewModel>>();
    }

    #endregion
}