using HaoKao.CourseService.Application.Modules.CourseChapterModule.Interfaces;
using HaoKao.CourseService.Application.Modules.CourseChapterModule.ViewModels;

namespace HaoKao.CourseService.Application.Modules.CourseChapterModule.Services;

/// <summary>
/// 课程章节接口服务
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.WeChat)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class CourseChapterWeChatService(ICourseChapterWebService service) : ICourseChapterWeChatService
{
    private readonly ICourseChapterWebService _service = service ?? throw new ArgumentNullException(nameof(service));

    /// <summary>
    /// 获取数据-章节树视频列表,这里需要将视频兼容进去
    /// </summary>
    /// <param name="courseId">课程id</param>
    /// <returns></returns>
    [HttpGet("tree")]
    public Task<List<dynamic>> GetTreeByQueryAsync(Guid? courseId)
    {
        return _service.GetTreeByQueryAsync(courseId);
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
        return _service.GetTreeAsync(id, courseId);
    }

    /// <summary>
    /// 按照课程id获取所有课程章节
    /// </summary>
    /// <param name="courseId">课程id</param>
    /// <returns></returns>
    [HttpGet("{courseId}")]
    public Task<List<BrowseCourseChapterViewModel>> GetAllAsync(Guid courseId)
    {
        return _service.GetAllAsync(courseId);
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
        return service.IsExistTry(courseIds);
    }
}