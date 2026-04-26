using HaoKao.CourseService.Application.Modules.CourseModule.ViewModels;

namespace HaoKao.CourseService.Application.Modules.CourseModule.Interfaces;

public interface ICourseService : IAppWebApiService, IManager
{
    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    Task<BrowseCourseViewModel> Get(Guid id);

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    Task<QueryCourseViewModel> Get(QueryCourseViewModel queryViewModel);

    /// <summary>
    /// 创建课程
    /// </summary>
    /// <param name="model">新增模型</param>
    Task Create(CreateCourseViewModel model);

    /// <summary>
    /// 根据主键删除指定课程
    /// </summary>
    /// <param name="id">主键</param>
    Task Delete(Guid id);

    /// <summary>
    /// 根据主键更新指定课程
    /// </summary>
    /// <param name="model">新增模型</param>
    Task Update(UpdateCourseViewModel model);

    /// <summary>
    /// 根据主键更新指定课程讲义包
    /// </summary>
    /// <param name="model">新增模型</param>
    Task UpdateCourseMaterialsPackageUrl([FromBody] UpdateCourseMaterialsPackageUrlViewModel model);

    /// <summary>
    /// 合并课程章节表和视频表
    /// </summary>
    /// <returns></returns>
    Task MergeChaperAndVideo();
}