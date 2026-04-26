using HaoKao.AgreementService.Application.ViewModels.CourseAgreement;

namespace HaoKao.AgreementService.Application.Services.Management;

public interface ICourseAgreementService : IAppWebApiService, IManager
{
    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    Task<BrowseCourseAgreementViewModel> Get(Guid id);

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="viewModel">查询对象</param>
    Task<QueryCourseAgreementViewModel> Get(QueryCourseAgreementViewModel viewModel);

    /// <summary>
    /// 根据ids获取列表
    /// </summary>
    /// <param name="ids">查询对象</param>
    Task<List<BrowseCourseAgreementViewModel>> GetByIds(IReadOnlyList<Guid> ids);

    /// <summary>
    /// 创建课程协议
    /// </summary>
    /// <param name="model">新增模型</param>
    Task Create(CreateCourseAgreementViewModel model);

    /// <summary>
    /// 根据主键删除指定课程协议
    /// </summary>
    /// <param name="id">主键</param>
    Task Delete(Guid id);

    /// <summary>
    /// 根据主键更新指定课程协议
    /// </summary>
    /// <param name="id">主键</param>
    /// <param name="model">新增模型</param>
    Task Update(Guid id, UpdateCourseAgreementViewModel model);
}