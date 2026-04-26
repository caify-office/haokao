using HaoKao.ContinuationService.Application.ContinuationAuditModule.ViewModels;
using HaoKao.ContinuationService.Domain.ContinuationAuditModule;

namespace HaoKao.ContinuationService.Application.ContinuationAuditModule.Interfaces;

public interface IContinuationAuditService : IAppWebApiService, IManager
{
    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    Task<BrowseContinuationAuditViewModel> Get(Guid id);

    /// <summary>
    /// 根据Id获取实体
    /// </summary>
    /// <param name="id">id</param>
    /// <returns></returns>
    Task<ContinuationAudit> GetEntity(Guid id);

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="viewModel">查询对象</param>
    Task<QueryContinuationAuditViewModel> Get(QueryContinuationAuditViewModel viewModel);

    /// <summary>
    /// 创建续读审核
    /// </summary>
    /// <param name="model">新增模型</param>
    Task Create(CreateContinuationAuditViewModel model);

    /// <summary>
    /// 根据主键更新指定续读审核
    /// </summary>
    /// <param name="model">新增模型</param>
    Task Update(UpdateContinuationAuditViewModel model);
}