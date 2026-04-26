using HaoKao.CampaignService.Application.ViewModels.GiftBag;

namespace HaoKao.CampaignService.Application.Services.WeChat;

public interface IGiftBagWeChatService : IAppWebApiService, IManager
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
}