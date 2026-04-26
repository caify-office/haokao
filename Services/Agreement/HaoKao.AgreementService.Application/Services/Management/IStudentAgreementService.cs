using HaoKao.AgreementService.Application.ViewModels.StudentAgreement;
using HaoKao.AgreementService.Domain.Entities;

namespace HaoKao.AgreementService.Application.Services.Management;

public interface IStudentAgreementService : IAppWebApiService, IManager
{
    /// <summary>
    /// 根据主键获取指定学员协议
    /// </summary>
    /// <param name="id">主键</param>
    Task<BrowseStudentAgreementViewModel> Get(Guid id);

    /// <summary>
    /// 根据主键获取指定学员协议
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<StudentAgreement> GetEntity(Guid id);

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    Task<QueryStudentAgreementViewModel> Get(QueryStudentAgreementViewModel queryViewModel);

    /// <summary>
    /// 创建学员协议
    /// </summary>
    /// <param name="model">新增模型</param>
    Task Create(CreateStudentAgreementViewModel model);

    /// <summary>
    /// 根据主键更新指定学员协议
    /// </summary>
    /// <param name="id">主键</param>
    /// <param name="model">新增模型</param>
    Task Update(Guid id, UpdateStudentAgreementViewModel model);
}