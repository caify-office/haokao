using HaoKao.CourseFeatureService.Application.ViewModels;

namespace HaoKao.CourseFeatureService.Application.Interfaces;

public interface ICourseFeatureService : IAppWebApiService, IManager
{
    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    Task<BrowseCourseFeatureViewModel> Get(Guid id);

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    Task<QueryCourseFeatureViewModel> Get(QueryCourseFeatureViewModel queryViewModel);

    /// <summary>
    /// 创建课程特色服务
    /// </summary>
    /// <param name="model">新增模型</param>
    Task Create(CreateCourseFeatureViewModel model);

    /// <summary>
    /// 批量删除课程特色服务
    /// </summary>
    /// <param name="ids">主键</param>
    Task Delete(List<Guid> ids);

    /// <summary>
    /// 根据主键更新指定课程特色服务
    /// </summary>
    /// <param name="model">更新模型</param>
    Task Update(UpdateCourseFeatureViewModel model);

    /// <summary>
    /// 批量启用/禁用课程特色服务
    /// </summary>
    /// <param name="model">启用/禁用模型</param>
    Task Enable(EnableCourseFeatureViewModel model);
}