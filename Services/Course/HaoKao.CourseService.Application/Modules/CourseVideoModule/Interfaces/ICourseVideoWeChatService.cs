using HaoKao.CourseService.Application.Modules.CourseVideoModule.ViewModels;

namespace HaoKao.CourseService.Application.Modules.CourseVideoModule.Interfaces;

public interface ICourseVideoWeChatService : IAppWebApiService, IManager
{
    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    Task<BrowseCourseVideoViewModel> Get(Guid id);

    /// <summary>
    /// 根据查询获取列表，用于分页(读取课程下面所有的课程视频id)
    /// </summary>
    /// <param name="queryViewModel"></param>
    /// <returns></returns>
    Task<QueryCourseVideoViewModel> GetCourseVideoList(QueryCourseVideoViewModel queryViewModel);

    /// <summary>
    /// 读取课程下面的更新资料接口
    /// </summary>
    /// <param name="CourseIds"></param>
    /// <returns></returns>
    Task<List<UpdateCourseVideoNewViewModel>> GetUpdateCourseVideoList([FromQuery] string CourseIds);

    /// <summary>
    /// 读取课程下面的更新资料接口(智辅课程专用)
    /// </summary>
    /// <param name="courseIds"></param>
    /// <returns></returns>
    Task<List<UpdateCourseVideoNewViewModel>> GetUpdateAssistantCourseVideoList([FromQuery] string courseIds);

    /// <summary>
    /// 根据章节id拉取章节下面的视频
    /// </summary>
    /// <param name="queryViewModel"></param>
    /// <returns></returns>
    Task<QueryCourseVideoViewModel> Get(QueryCourseVideoViewModel queryViewModel);

    /// <summary>
    /// 通过知识点Id获取关联知识点对应的学习视频
    /// </summary>
    /// <param name="CourseChapterId">课程id</param>
    /// <param name="knowledgePointIds">知识点id数组</param>
    /// <returns></returns>
    Task<List<BrowseCourseVideoViewModel>> GetVideoInfoByKnowledgePointId(Guid? CourseChapterId, List<Guid> knowledgePointIds);
}