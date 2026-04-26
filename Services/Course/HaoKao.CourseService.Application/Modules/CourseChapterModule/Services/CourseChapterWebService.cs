using HaoKao.CourseService.Application.Modules.CourseChapterModule.Interfaces;
using HaoKao.CourseService.Application.Modules.CourseChapterModule.ViewModels;
using HaoKao.CourseService.Domain.CourseChapterModule;

namespace HaoKao.CourseService.Application.Modules.CourseChapterModule.Services;

/// <summary>
/// 课程章节接口服务
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.WebSite)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class CourseChapterWebService(ICourseChapterRepository repository, ICourseChapterService courseChapterService) : ICourseChapterWebService
{
    private readonly ICourseChapterRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));

    /// <summary>
    /// 获取数据-章节树视频列表,这里需要将视频兼容进去
    /// </summary>
    /// <param name="courseId">课程id</param>
    /// <returns></returns>
    [HttpGet("tree")]
    public Task<List<dynamic>> GetTreeByQueryAsync(Guid? courseId)
    {
        return _repository.GetChapterVideoTreeByQueryAsync(courseId);
    }

    /// <summary>
    /// 获取数据-章节树列表根据课程id获取章节树不兼容视频
    /// </summary>
    /// <param name="id">展开的节点</param>
    /// <param name="courseId">课程id</param>
    /// <returns></returns>
    [HttpGet("tree")]
    public Task<List<dynamic>> GetTreeAsync(Guid? id, Guid? courseId)
    {
        return _repository.GetChapterNodeTreeByQueryAsync(id, courseId);
    }

    /// <summary>
    /// 按照课程id获取所有课程章节
    /// </summary>
    /// <param name="courseId">课程id</param>
    /// <returns></returns>
    [HttpGet("{courseId:guid}")]
    public Task<List<BrowseCourseChapterViewModel>> GetAllAsync(Guid courseId)
    {
        return courseChapterService.GetAllAsync(courseId);
    }

    /// <summary>
    /// 判断这些课程下是否有试听视频
    /// </summary>
    /// <param name="courseIds"></param>
    /// <returns></returns>
    /// <returns></returns>
    [AllowAnonymous]
    public Task<bool> IsExistTry(Guid[] courseIds)
    {
        return _repository.IsExistTry(courseIds);
    }
}