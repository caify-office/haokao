using HaoKao.CampaignService.Application.ViewModels.GiftBagReceiveLog;
using HaoKao.CampaignService.Domain.Entities;
using HaoKao.CampaignService.Domain.Queries;
using HaoKao.CampaignService.Domain.Repositories;

namespace HaoKao.CampaignService.Application.Services.Management;

/// <summary>
/// 礼包领取记录-管理端接口
/// </summary>
/// <param name="cacheManager"></param>
/// <param name="repository"></param>
[DynamicWebApi(Module = ServiceAreaName.Management)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsJwt)]
public class GiftBagReceiveLogService(IStaticCacheManager cacheManager, IGiftBagReceiveLogRepository repository) : IGiftBagReceiveLogService
{
    private readonly IStaticCacheManager _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
    private readonly IGiftBagReceiveLogRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    /// <returns></returns>
    [HttpGet]
    public async Task<QueryGiftBagReceiveLogViewModel> Get([FromQuery] QueryGiftBagReceiveLogViewModel queryViewModel)
    {
        var query = queryViewModel.MapToQuery<GiftBagReceiveLogQuery>();
        query.OrderBy = nameof(GiftBagReceiveLog.ReceiveTime);
        var tempQuery = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<GiftBagReceiveLog>.QueryCacheKey.Create(query.GetCacheKey()),
            async () =>
            {
                await _repository.GetByQueryAsync(query);
                return query;
            });

        if (!query.Equals(tempQuery))
        {
            query.RecordCount = tempQuery.RecordCount;
            query.Result = tempQuery.Result;
        }

        return query.MapToQueryDto<QueryGiftBagReceiveLogViewModel, GiftBagReceiveLog>();
    }
}