using HaoKao.ContinuationService.Application.ProductExtensionModule.Interfaces;
using HaoKao.ContinuationService.Application.ProductExtensionModule.ViewModels;
using HaoKao.ContinuationService.Application.ProductExtensionRequestModule.ViewModels;

namespace HaoKao.ContinuationService.Application.ProductExtensionModule.Services;

/// <summary>
/// 服务申请接口 (微信端)
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.WebSite)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class ProductExtensionWeChatService(IProductExtensionWebService service) : IProductExtensionWeChatService
{
    private readonly IProductExtensionWebService _service = service ?? throw new ArgumentNullException(nameof(service));

    /// <summary>
    /// 查看详情
    /// </summary>
    /// <param name="id">主键</param>
    [HttpGet("{id:guid}")]
    public Task<BrowseProductExtensionRequestWebViewModel> Get(Guid id)
    {
        return _service.Get(id);
    }

    /// <summary>
    /// 可申请列表
    /// </summary>
    /// <param name="productIds"></param>
    /// <returns></returns>
    [HttpPost]
    public Task<List<ProductExtensionServiceRequestViewModel>> GetRequestList([FromBody] List<Guid> productIds)
    {
        return _service.GetRequestList(productIds);
    }

    /// <summary>
    /// 申请记录列表
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public Task<List<QueryProductExtensionRequestListWebViewModel>> GetRequestRecord()
    {
        return _service.GetRequestRecord();
    }

    /// <summary>
    /// 申请续读
    /// </summary>
    /// <param name="model">新增模型</param>
    [HttpPost]
    public Task Create([FromBody] CreateProductExtensionRequestViewModel model)
    {
        return _service.Create(model);
    }
}