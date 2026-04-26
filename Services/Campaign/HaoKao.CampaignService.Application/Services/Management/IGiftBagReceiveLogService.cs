using HaoKao.CampaignService.Application.ViewModels.GiftBagReceiveLog;

namespace HaoKao.CampaignService.Application.Services.Management;

public interface IGiftBagReceiveLogService : IAppWebApiService, IManager
{
    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    /// <returns></returns>
    Task<QueryGiftBagReceiveLogViewModel> Get(QueryGiftBagReceiveLogViewModel queryViewModel);
}