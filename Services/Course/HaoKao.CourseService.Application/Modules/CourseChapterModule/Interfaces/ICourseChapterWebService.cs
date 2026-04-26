using HaoKao.CourseService.Application.Modules.CourseChapterModule.ViewModels;

namespace HaoKao.CourseService.Application.Modules.CourseChapterModule.Interfaces;

public interface ICourseChapterWebService : IAppWebApiService, IManager
{
    Task<List<dynamic>> GetTreeByQueryAsync(Guid? courseId);

    Task<List<dynamic>> GetTreeAsync(Guid? id, Guid? courseId);

    /// <summary>
    /// 按照课程id获取所有课程章节
    /// </summary>
    /// <param name="courseId">课程id</param>
    /// <returns></returns>
    Task<List<BrowseCourseChapterViewModel>> GetAllAsync(Guid courseId);

    Task<bool> IsExistTry(Guid[] courseIds);
}