using HaoKao.CourseService.Application.Modules.CourseMaterialsModule.ViewModels;

namespace HaoKao.CourseService.Application.Modules.CourseMaterialsModule.Interfaces;

public interface ICourseMaterialsService : IAppWebApiService, IManager
{
    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    Task<BrowseCourseMaterialsViewModel> Get(Guid id);

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    Task<QueryCourseMaterialsViewModel> Get(QueryCourseMaterialsViewModel queryViewModel);

    /// <summary>
    /// 创建课程讲义
    /// </summary>
    /// <param name="model">新增模型</param>
    Task Create(CreateCourseMaterialsViewModel model);

    /// <summary>
    /// 保存课程讲义（智辅学习专用）
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task Save([FromBody] SaveCourseMaterialsViewModel model);

    /// <summary>
    /// 根据主键删除指定课程讲义
    /// </summary>
    /// <param name="id">主键</param>
    Task Delete(Guid id);

    /// <summary>
    /// 根据主键更新指定课程讲义
    /// </summary>
    /// <param name="id">主键</param>
    /// <param name="model">新增模型</param>
    Task Update(Guid id, SetCourseMaterialsSortViewModel model);
}