using HaoKao.CampaignService.Domain.Entities;

namespace HaoKao.CampaignService.Application.ViewModels.GiftBag;

/// <summary>
/// 获取可领取的礼包图片
/// </summary>
/// <param name="Id">礼包Id</param>
/// <param name="ImageSet">礼包图片</param>
/// <param name="Receivable">是否可以领取</param>
public record GetGiftBagImageSetViewModel(Guid Id, GiftBagImageSet ImageSet, bool Receivable) : IDto;

/// <summary>
/// 领取礼包参数
/// </summary>
/// <param name="UserName">用户昵称</param>
/// <param name="RegistrationTime">注册时间</param>
public record ReceiveGiftBagViewModel(string UserName, DateTime RegistrationTime) : IDto;