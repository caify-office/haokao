using HaoKao.ContinuationService.Application.ContinuationAuditModule.Interfaces;
using HaoKao.ContinuationService.Application.ContinuationAuditModule.ViewModels;
using HaoKao.ContinuationService.Application.ContinuationModule.Interfaces;
using HaoKao.ContinuationService.Application.ContinuationModule.ViewModels;
using HaoKao.ContinuationService.Domain.ContinuationAuditModule;
using HaoKao.ContinuationService.Domain.ContinuationSetupModule;

namespace HaoKao.ContinuationService.Application.ContinuationModule.Services;

/// <summary>
/// 服务申请接口
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.WebSite)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class ContinuationWebService(
    IContinuationSetupRepository continuationSetupRepository,
    IContinuationAuditRepository continuationAuditRepository,
    IContinuationAuditService continuationAuditService,
    IStaticCacheManager cacheManager
) : IContinuationWebService
{
    #region 初始参数

    private readonly IContinuationSetupRepository _continuationSetupRepository = continuationSetupRepository ?? throw new ArgumentNullException(nameof(continuationSetupRepository));
    private readonly IContinuationAuditRepository _continuationAuditRepository = continuationAuditRepository ?? throw new ArgumentNullException(nameof(continuationAuditRepository));
    private readonly IContinuationAuditService _continuationAuditService = continuationAuditService ?? throw new ArgumentNullException(nameof(continuationAuditService));
    private readonly IStaticCacheManager _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));

    #endregion

    #region 服务方法

    /// <summary>
    /// 查看详情
    /// </summary>
    /// <param name="id">主键</param>
    [HttpGet("{id:guid}")]
    public async Task<BrowseContinuationAuditWebViewModel> Get(Guid id)
    {
        var entity = await _continuationAuditService.GetEntity(id);
        return entity.MapToDto<BrowseContinuationAuditWebViewModel>();
    }

    /// <summary>
    /// 可申请列表
    /// </summary>
    /// <param name="productIds"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<List<ServiceRequestViewModel>> GetRequestList([FromBody] List<Guid> productIds)
    {
        // 获取当前时间段内可以申请的续读配置
        var queryable = _continuationSetupRepository.Query.Where(
            x => x.Enable && x.StartTime <= DateTime.Now && x.EndTime >= DateTime.Now
        );
        var key = queryable.ToQueryString().ToMd5();
        var setups = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<ContinuationSetup>.QueryCacheKey.Create(key),
            () => queryable.ToListAsync()
        );

        // 查询已申请次数(申请成功)
        var auditList = await GetContinuationAudits();

        var list = new List<ServiceRequestViewModel>();
        foreach (var setup in setups)
        {
            foreach (var p in setup.Products)
            {
                // 未开放申请的产品不显示
                if (!productIds.Contains(p.ProductId)) continue;

                // 当前配置已申请的不显示
                if (auditList.Any(x => x.ProductId == p.ProductId && x.SetupId == setup.Id)) continue;

                // 协议可申请的次数
                var continuation = setup.Products.First(x => x.ProductId == p.ProductId).Continuation;

                // 已申请成功的次数
                var applyCount = auditList.Count(x => x.AuditState == AuditState.Pass && x.ProductId == p.ProductId);

                // 剩余可申请的次数
                var restOfCount = continuation - applyCount;

                if (restOfCount > 0)
                {
                    list.Add(new()
                    {
                        ProductId = p.ProductId,
                        SetupId = setup.Id,
                        RestOfCount = restOfCount
                    });
                }
            }
        }

        return list;
    }

    /// <summary>
    /// 申请记录列表
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<List<ContinuationAudit>> GetRequestRecord()
    {
        // 查询已申请次数(申请成功)
        var list = await GetContinuationAudits();

        // 根据setupId 分组, 取最新的数据
        return list.GroupBy(x => new { x.SetupId, x.ProductId })
                   .Select(g => g.MaxBy(x => x.CreateTime))
                   .OrderByDescending(x => x.CreateTime)
                   .ToList();
    }

    /// <summary>
    /// 申请续读
    /// </summary>
    /// <param name="model">新增模型</param>
    [HttpPost]
    public Task Create([FromBody] CreateContinuationAuditViewModel model)
    {
        return _continuationAuditService.Create(model);
    }

    private async Task<List<ContinuationAudit>> GetContinuationAudits()
    {
        var userId = EngineContext.Current.ClaimManager.GetUserId().To<Guid>();
        var query = new ContinuationAuditQuery { CreatorId = userId };
        var list = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<ContinuationAudit>.QueryCacheKey.Create(query.GetCacheKey()),
            () => _continuationAuditRepository.GetWhereAsync(query.GetQueryWhere())
        );
        return list;
    }

    #endregion
}