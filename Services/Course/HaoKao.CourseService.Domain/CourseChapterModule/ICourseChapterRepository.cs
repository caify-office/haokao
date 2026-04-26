namespace HaoKao.CourseService.Domain.CourseChapterModule;

public interface ICourseChapterRepository : IRepository<CourseChapter>
{
    /// <summary>
    /// 清空课程章节
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<int> ClearCourseChapter(Guid id);

    /// <summary>
    /// 课程章节树形
    /// </summary>
    /// <param name="id"></param>
    /// <param name="courseId"></param>
    /// <returns></returns>
    Task<List<dynamic>> GetChapterNodeTreeByQueryAsync(Guid? id, Guid? courseId);

    /// <summary>
    /// 章节视频树形列表,需要将视频也兼容进去-只保留一级
    /// </summary>
    /// <param name="courseId">课程id</param>
    /// <returns></returns>
    Task<List<dynamic>> GetChapterVideoTreeByQueryAsync(Guid? courseId);

    /// <summary>
    /// 判断这些课程下是否有试听视频
    /// </summary>
    /// <param name="courseIds"></param>
    /// <returns></returns>
    Task<bool> IsExistTry(Guid[] courseIds);
}