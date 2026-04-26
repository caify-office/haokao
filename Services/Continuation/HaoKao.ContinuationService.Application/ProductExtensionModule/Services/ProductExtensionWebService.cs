using HaoKao.ContinuationService.Application.ProductExtensionModule.Interfaces;
using HaoKao.ContinuationService.Application.ProductExtensionModule.ViewModels;
using HaoKao.ContinuationService.Application.ProductExtensionRequestModule.Interfaces;
using HaoKao.ContinuationService.Application.ProductExtensionRequestModule.ViewModels;
using HaoKao.ContinuationService.Domain.ProductExtensionPolicyModule;
using HaoKao.ContinuationService.Domain.ProductExtensionRequestModule;

namespace HaoKao.ContinuationService.Application.ProductExtensionModule.Services;

/// <summary>
/// 服务申请接口 (Web端)
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.WebSite)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class ProductExtensionWebService(
    IProductExtensionPolicyRepository policyRepository,
    IProductExtensionRequestRepository requestRepository,
    IProductExtensionRequestService requestService,
    IProductExtensionDomainService domainService,
    IStaticCacheManager cacheManager,
    IMapper mapper
) : IProductExtensionWebService
{
    private readonly IProductExtensionPolicyRepository _policyRepository = policyRepository ?? throw new ArgumentNullException(nameof(policyRepository));
    private readonly IProductExtensionRequestRepository _requestRepository = requestRepository ?? throw new ArgumentNullException(nameof(requestRepository));
    private readonly IProductExtensionRequestService _requestService = requestService ?? throw new ArgumentNullException(nameof(requestService));
    private readonly IProductExtensionDomainService _domainService = domainService ?? throw new ArgumentNullException(nameof(domainService));
    private readonly IStaticCacheManager _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

    /// <summary>
    /// 查看详情
    /// </summary>
    /// <param name="id">主键</param>
    [HttpGet("{id:guid}")]
    public async Task<BrowseProductExtensionRequestWebViewModel> Get(Guid id)
    {
        var request = await _requestService.Get(id);
        return _mapper.Map<BrowseProductExtensionRequestWebViewModel>(request);
    }

    /// <summary>
    /// 可申请列表
    /// </summary>
    /// <param name="productIds">产品Id集合</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<List<ProductExtensionServiceRequestViewModel>> GetRequestList([FromBody] List<Guid> productIds)
    {
        // 1. 获取当前时间段内开启的、有效的续读策略
        var now = DateTime.Now;
        var queryable = _policyRepository.Query.Where(x => x.IsEnable && x.StartTime <= now && x.EndTime >= now);
        var key = queryable.ToQueryString().ToMd5();
        var policies = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<ProductExtensionPolicy>.QueryCacheKey.Create(key),
            () => queryable.ToListAsync()
        );

        // 2. 获取当前用户的所有申请记录 (用于计算剩余次数)
        var userRequests = await GetCurrentUserRequests();

        var result = new List<ProductExtensionServiceRequestViewModel>();

        foreach (var policy in policies)
        {
            foreach (var product in policy.Products)
            {
                // 如果该产品不在用户传入的列表中，跳过
                if (!productIds.Contains(product.ProductId)) continue;

                // 调用 Domain Service 判断是否有资格申请
                if (!_domainService.CanMakeRequest(policy, product, userRequests)) continue;

                // 调用 Domain Service 计算剩余次数
                var restOfCount = _domainService.CalculateRestCount(product, userRequests);

                result.Add(new ProductExtensionServiceRequestViewModel
                {
                    ProductId = product.ProductId,
                    PolicyId = policy.Id,
                    RestOfCount = restOfCount
                });
            }
        }

        return result;
    }

    /// <summary>
    /// 申请记录列表
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<List<QueryProductExtensionRequestListWebViewModel>> GetRequestRecord()
    {
        var list = await GetCurrentUserRequests();

        var groupedList = list.GroupBy(x => new { x.PolicyId, x.ProductId })
                              .Select(g => g.MaxBy(x => x.CreateTime))
                              .OrderByDescending(x => x.CreateTime)
                              .ToList();

        return _mapper.Map<List<QueryProductExtensionRequestListWebViewModel>>(groupedList);
    }

    /// <summary>
    /// 创建续读申请
    /// </summary>
    /// <param name="model">新增模型</param>
    [HttpPost]
    public Task Create([FromBody] CreateProductExtensionRequestViewModel model)
    {
        return _requestService.Create(model);
    }

    private async Task<List<ProductExtensionRequest>> GetCurrentUserRequests()
    {
        var userId = EngineContext.Current.ClaimManager.GetUserId().To<Guid>();
        var query = _requestRepository.Query.Where(x => x.CreatorId == userId);

        // 缓存处理
        var key = query.ToQueryString().ToMd5();
        var list = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<ProductExtensionRequest>.QueryCacheKey.Create(key),
            () => query.ToListAsync()
        );
        return list;
    }
}