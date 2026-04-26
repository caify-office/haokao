using HaoKao.ContinuationService.Application.ContinuationAuditModule.ViewModels;
using HaoKao.ContinuationService.Application.ContinuationModule.ViewModels;
using HaoKao.ContinuationService.Domain.ContinuationAuditModule;

namespace HaoKao.ContinuationService.Application.ContinuationModule.Interfaces;

public interface IContinuationWeChatService : IAppWebApiService, IManager
{
    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    Task<BrowseContinuationAuditWebViewModel> Get(Guid id);

    /// <summary>
    /// 可申请列表
    /// </summary>
    /// <param name="productIds"></param>
    /// <returns></returns>
    Task<List<ServiceRequestViewModel>> GetRequestList(List<Guid> productIds);

    /// <summary>
    /// 申请记录列表
    /// </summary>
    /// <returns></returns>
    Task<List<ContinuationAudit>> GetRequestRecord();

    /// <summary>
    /// 创建续读审核
    /// </summary>
    /// <param name="model">新增模型</param>
    Task Create(CreateContinuationAuditViewModel model);
}