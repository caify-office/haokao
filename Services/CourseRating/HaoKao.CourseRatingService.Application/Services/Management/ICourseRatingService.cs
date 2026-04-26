using HaoKao.CourseRatingService.Application.ViewModels;
using HaoKao.CourseRatingService.Domain.Enums;

namespace HaoKao.CourseRatingService.Application.Services.Management;

public interface ICourseRatingService : IAppWebApiService, IManager
{
    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    Task<BrowseCourseRatingViewModel> Get(Guid id);

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    Task<QueryCourseRatingViewModel> Get(QueryCourseRatingViewModel queryViewModel);

    /// <summary>
    /// 创建课程评价
    /// </summary>
    /// <param name="model">新增模型</param>
    Task Create(CreateCourseRatingViewModel model);

    /// <summary>
    /// 根据主键删除指定课程评价
    /// </summary>
    /// <param name="id">主键</param>
    Task Delete(Guid id);

    /// <summary>
    /// 审核课程评价
    /// </summary>
    /// <param name="id">主键</param>
    /// <param name="state">审核状态</param>
    Task Audit(Guid id, AuditState state);

    /// <summary>
    /// 置顶课程评价
    /// </summary>
    /// <param name="id">主键</param>
    /// <param name="sticky">是否置顶</param>
    /// <returns></returns>
    Task Sticky(Guid id, bool sticky);
}