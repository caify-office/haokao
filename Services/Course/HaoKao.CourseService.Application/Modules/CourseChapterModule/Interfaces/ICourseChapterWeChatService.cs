using HaoKao.CourseService.Application.Modules.CourseChapterModule.ViewModels;

namespace HaoKao.CourseService.Application.Modules.CourseChapterModule.Interfaces;

public interface ICourseChapterWeChatService : IAppWebApiService, IManager
{
    /// <summary>
    /// 获取数据-章节树视频列表,这里需要将视频兼容进去
    /// </summary>
    /// <param name="courseId"></param>
    /// <returns></returns>
    Task<List<dynamic>> GetTreeByQueryAsync(Guid? courseId);

    /// <summary>
    /// 获取数据-章节树列表根据课程id获取章节树不兼容视频
    /// </summary>
    /// <param name="id"></param>
    /// <param name="courseId"></param>
    /// <returns></returns>
    Task<List<dynamic>> GetTreeAsync(Guid? id, Guid? courseId);

    /// <summary>
    /// 按照课程id获取所有课程章节
    /// </summary>
    /// <param name="courseId">课程id</param>
    /// <returns></returns>
    Task<List<BrowseCourseChapterViewModel>> GetAllAsync(Guid courseId);

    Task<bool> IsExistTry(Guid[] courseIds);
}