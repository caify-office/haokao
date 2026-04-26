using HaoKao.CourseService.Application.Modules.CourseModule.ViewModels;

namespace HaoKao.CourseService.Application.Modules.CourseModule.Interfaces;

public interface ICourseWebService : IAppWebApiService, IManager
{
    /// <summary>
    /// 课程列表
    /// </summary>
    /// <param name="queryViewModel"></param>
    /// <returns></returns>
    Task<QueryCourseViewModel> Get(QueryCourseViewModel queryViewModel);

    /// <summary>
    /// 读取讲师集合
    /// </summary>
    /// <param name="queryViewModel"></param>
    /// <returns></returns>
    Task<List<string>> GetTeacherIds(QueryCourseViewModel queryViewModel);

    /// <summary>
    /// 根据课程ids集合批量读取课程集合信息
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    Task<List<BrowseCourseViewModel>> GetCourseInfos(string ids);

    /// <summary>
    /// 读取课程详情信息
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<BrowseCourseViewModel> GetCourseInfo(Guid id);
}