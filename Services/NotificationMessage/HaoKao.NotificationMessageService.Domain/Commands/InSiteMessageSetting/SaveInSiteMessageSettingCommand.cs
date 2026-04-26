using HaoKao.NotificationMessageService.Domain.Models;

namespace HaoKao.NotificationMessageService.Domain.Commands.InSiteMessageSetting;
/// <summary>
/// 修改站内消息配置
/// </summary>
/// <param name="Templates">消息模板集合</param>
public record SaveInSiteMessageSettingCommand(List<MessageTemplate> Templates) : Command("修改站内消息配置");