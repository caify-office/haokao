using HaoKao.ContinuationService.Application.ContinuationAuditModule.ViewModels;
using HaoKao.ContinuationService.Application.ContinuationModule.Interfaces;
using HaoKao.ContinuationService.Application.ContinuationModule.ViewModels;
using HaoKao.ContinuationService.Domain.ContinuationAuditModule;

namespace HaoKao.ContinuationService.Application.ContinuationModule.Services;

/// <summary>
/// 服务申请接口
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.WebSite)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class ContinuationWeChatService(IContinuationWebService service) : IContinuationWeChatService
{
    /// <summary>
    /// 查看详情
    /// </summary>
    /// <param name="id">主键</param>
    [HttpGet("{id:guid}")]
    public Task<BrowseContinuationAuditWebViewModel> Get(Guid id)
    {
        return service.Get(id);
    }

    /// <summary>
    /// 可申请列表
    /// </summary>
    /// <param name="productIds"></param>
    /// <returns></returns>
    [HttpPost]
    public Task<List<ServiceRequestViewModel>> GetRequestList([FromBody] List<Guid> productIds)
    {
        return service.GetRequestList(productIds);
    }

    /// <summary>
    /// 申请记录列表
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public Task<List<ContinuationAudit>> GetRequestRecord()
    {
        return service.GetRequestRecord();
    }

    /// <summary>
    /// 申请续读
    /// </summary>
    /// <param name="model">新增模型</param>
    [HttpPost]
    public Task Create([FromBody] CreateContinuationAuditViewModel model)
    {
        return service.Create(model);
    }
}