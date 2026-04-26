using HaoKao.NotificationMessageService.Domain.Models;

namespace HaoKao.NotificationMessageService.Domain.Commands.WechatMessageSetting;
/// <summary>
/// 修改微信模板消息配置
/// </summary>
/// <param name="AppId">AppId</param>
/// <param name="AppSecret">AppSecret</param>
/// <param name="Templates">消息模板集合</param>
public record SaveWechatMessageSettingCommand(string AppId, string AppSecret,
    List<MessageTemplate> Templates) : Command("修改微信模板消息配置");