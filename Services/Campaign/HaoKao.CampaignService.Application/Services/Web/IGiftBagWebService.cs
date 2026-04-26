using HaoKao.CampaignService.Application.ViewModels.GiftBag;
using HaoKao.CampaignService.Domain.Entities;
using HaoKao.CampaignService.Domain.Enums;

namespace HaoKao.CampaignService.Application.Services.Web;

public interface IGiftBagWebService : IAppWebApiService, IManager
{

    /// <summary>
    /// 按Id获取礼包
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<GetGiftBagImageSetViewModel> Get(Guid id);

    /// <summary>
    /// 获取用户可领取的礼包列表
    /// </summary>
    /// <param name="registrationTime"></param>
    /// <returns>礼包配置的图片</returns>
    Task<IReadOnlyList<GetGiftBagImageSetViewModel>> Get(DateTime registrationTime);

    /// <summary>
    /// 领取礼包
    /// </summary>
    /// <param name="id">礼包Id</param>
    /// <param name="model"></param>
    /// <returns></returns>
    Task Receive(Guid id, ReceiveGiftBagViewModel model);

    /// <summary>
    /// 获取礼包图片
    /// </summary>
    /// <param name="receivePlatform"></param>
    /// <param name="registrationTime"></param>
    /// <returns></returns>
    Task<IReadOnlyList<GetGiftBagImageSetViewModel>> GetGiftBagImageSetViewModel
        (ReceivePlatform receivePlatform, DateTime registrationTime = default);

    /// <summary>
    /// 从缓存中获取礼包列表
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<GiftBag> GetFromCache(Guid id);

    /// <summary>
    /// 设置领取平台
    /// </summary>
    /// <param name="platform"></param>
    void SetPlatform(ReceivePlatform platform);
}