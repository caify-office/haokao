using HaoKao.CampaignService.Application.Services.Web;
using HaoKao.CampaignService.Application.ViewModels.GiftBag;
using HaoKao.CampaignService.Domain.Enums;

namespace HaoKao.CampaignService.Application.Services.WeChat;

/// <summary>
/// 活动管理-微信端接口
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.WeChat)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class GiftBagWeChatService(IGiftBagWebService service) : IGiftBagWeChatService
{
    private readonly IGiftBagWebService _service = service ?? throw new ArgumentNullException(nameof(service));

    /// <summary>
    /// 按Id获取礼包
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id:guid}"), AllowAnonymous]
    public async Task<GetGiftBagImageSetViewModel> Get(Guid id)
    {
        var entity = await _service.GetFromCache(id);
        return new GetGiftBagImageSetViewModel(entity.Id, entity.WeChatMiniProgramImageSet, true);
    }

    /// <summary>
    /// 获取用户可领取的礼包列表
    /// </summary>
    /// <param name="registrationTime"></param>
    /// <returns>礼包配置的图片</returns>
    [HttpGet, AllowAnonymous]
    public Task<IReadOnlyList<GetGiftBagImageSetViewModel>> Get([FromQuery] DateTime registrationTime)
    {
        return _service.GetGiftBagImageSetViewModel(ReceivePlatform.WeChatMiniProgram, registrationTime);
    }

    /// <summary>
    /// 领取礼包
    /// </summary>
    /// <param name="id">礼包Id</param>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost("{id:guid}")]
    public Task Receive(Guid id, [FromBody] ReceiveGiftBagViewModel model)
    {
        _service.SetPlatform(ReceivePlatform.WeChatMiniProgram);
        return _service.Receive(id, model);
    }
}