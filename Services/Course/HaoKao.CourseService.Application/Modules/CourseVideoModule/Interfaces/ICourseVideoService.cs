using HaoKao.CourseService.Application.Modules.CourseVideoModule.ViewModels;

namespace HaoKao.CourseService.Application.Modules.CourseVideoModule.Interfaces;

public interface ICourseVideoService : IAppWebApiService, IManager
{
    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    Task<BrowseCourseVideoViewModel> Get(Guid id);

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    Task<QueryCourseVideoViewModel> Get(QueryCourseVideoViewModel queryViewModel);

    /// <summary>
    /// 读取课程下面的更新资料接口
    /// </summary>
    /// <param name="courseIds"></param>
    /// <returns></returns>
    Task<List<UpdateCourseVideoNewViewModel>> GetUpdateCourseVideoList(string courseIds);

    /// <summary>
    /// 读取课程下面的更新资料接口(智辅课程专用)
    /// </summary>
    /// <param name="courseIds"></param>
    /// <returns></returns>
    Task<List<UpdateCourseVideoNewViewModel>> GetUpdateAssistantCourseVideoList(string courseIds);

    /// <summary>
    /// 根据课程ids拿到多个课程下面所有的
    /// </summary>
    /// <param name="courseIds"></param>
    /// <returns></returns>
    Task<List<CourseVideoQueryListViewModel>> GetVideoIdsByCourseIds(string courseIds);

    /// <summary>
    /// 补充智辅课程知识点信息
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<QueryKnowledgePointConfigInfoViewModel> QueryKnowledgePointConfigInfo(QueryKnowledgePointConfigInfoViewModel model);

    /// <summary>
    /// 创建课程视频
    /// </summary>
    /// <param name="model">新增模型</param>
    Task Save(SaveCourseVideoViewModel model);

    /// <summary>
    /// 根据主键删除指定课程视频
    /// </summary>
    /// <param name="id">主键</param>
    Task Delete(Guid id);

    /// <summary>
    /// 根据主键更新指定课程视频
    /// </summary>
    /// <param name="id">主键</param>
    /// <param name="model">新增模型</param>
    Task Update(Guid id, UpdateCourseVideoViewModel model);

    /// <summary>
    /// 修改排序
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task UpdateSort(UpdateSortModel request);

    /// <summary>
    /// 修改知识点
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task UpdateKnowledgePoint(UpdateKnowledgePointModel request);
}