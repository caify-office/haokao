using Girvs.AuthorizePermission;
using Girvs.Extensions;
using Girvs.Infrastructure;
using HaoKao.Common;
using HaoKao.LiveBroadcastService.Application.Services.Management;
using HaoKao.LiveBroadcastService.Application.ViewModels.LiveReservation;
using HaoKao.LiveBroadcastService.Domain.CacheKeyManager;
using HaoKao.LiveBroadcastService.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using System.Linq;

namespace HaoKao.LiveBroadcastService.Application.Services.Web;

/// <summary>
/// 直播预约接口服务-Web端
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.WebSite)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class LiveReservationWebService(ILiveReservationService service, ILiveReservationRepository repository, IStaticCacheManager cacheManager)
    : ILiveReservationWebService
{
    private readonly ILiveReservationRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly IStaticCacheManager _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));

    /// <summary>
    /// 创建直播预约
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost]
    public Task Create([FromBody] CreateLiveReservationViewModel model)
    {
        model.ReservationSource = ReservationSource.WebSite;
        return service.Create(model);
    }

    /// <summary>
    /// 我的直播预约
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<List<BrowseLiveReservationViewModel>> MyLiveReservation()
    {
        var userId = EngineContext.Current.ClaimManager.GetUserId().ToGuid();
        var result = await _cacheManager.GetAsync(
            LiveReservationCacheKeyManager.CreateMyLiveReservationCacheKey(userId),
            () => _repository.GetWhereAsync(x => x.CreatorId == userId)
        );
        return result.MapTo<List<BrowseLiveReservationViewModel>>();
    }

    /// <summary>
    /// 预约人数统计
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [AllowAnonymous]
    public async Task<dynamic> LiveReservationCount(Guid[] productIds)
    {
        var result = await _repository.LiveReservationCount(productIds);
        productIds.ToList().ForEach(x =>
        {
            if (!result.ContainsKey(x))
            {
                result.Add(x, 0);
            }
        });
        return result;
    }
}