using HaoKao.CourseService.Application.Modules.CourseVideoModule.ViewModels;

namespace HaoKao.CourseService.Application.Modules.CourseVideoModule.Interfaces;

public interface ICourseVideoWebService : IAppWebApiService, IManager
{
    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    Task<BrowseCourseVideoViewModel> Get(Guid id);

    /// <summary>
    /// 读取课程下面的视频集合
    /// </summary>
    /// <param name="queryViewModel"></param>
    /// <returns></returns>
    Task<QueryCourseVideoViewModel> GetCourseVideoList(QueryCourseVideoViewModel queryViewModel);

    /// <summary>
    /// 读取课程视频详情
    /// </summary>
    /// <param name="queryViewModel"></param>
    /// <returns></returns>
    Task<QueryCourseVideoViewModel> Get(QueryCourseVideoViewModel queryViewModel);

    /// <summary>
    /// 根据课程ids读取视频ids集合
    /// </summary>
    /// <param name="CourseIds"></param>
    /// <returns></returns>
    Task<List<CourseVideoQueryListViewModel>> GetVideoIdsByCourseIds(string CourseIds);

    /// <summary>
    /// 读取课程下面的更新资料接口(智辅课程专用)
    /// </summary>
    /// <param name="courseIds"></param>
    /// <returns></returns>
    Task<List<UpdateCourseVideoNewViewModel>> GetUpdateAssistantCourseVideoList(string courseIds);

    /// <summary>
    /// 批量读取视频信息
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<List<BrowseCourseVideoViewModel>> GetVideoInfo(QueryByIds model);

    /// <summary>
    /// 读取课程下面的更新资料接口
    /// </summary>
    /// <param name="CourseIds"></param>
    /// <returns></returns>
    Task<List<UpdateCourseVideoNewViewModel>> GetUpdateCourseVideoList(string CourseIds);

    /// <summary>
    /// 通过知识点Id获取关联知识点对应的学习视频
    /// </summary>
    /// <param name="CourseChapterId">课程id</param>
    /// <param name="knowledgePointIds">知识点id数组</param>
    /// <returns></returns>
    Task<List<BrowseCourseVideoViewModel>> GetVideoInfoByKnowledgePointId(Guid? CourseChapterId, List<Guid> knowledgePointIds);
}