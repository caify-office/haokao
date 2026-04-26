using HaoKao.NotificationMessageService.Domain.Enumerations;
using HaoKao.NotificationMessageService.Domain.Models;

namespace HaoKao.NotificationMessageService.Domain.Commands.MobileMessageSetting;
/// <summary>
/// 修改手机短信配置
/// </summary>
/// <param name="MobileMessagePlatform">短信平台类型</param>
/// <param name="AppId"></param>
/// <param name="AppSecret">AppSecret</param>
/// <param name="SignList">签名列表</param>
/// <param name="DefaultSign">默认签名</param>
/// <param name="Templates">模板列表</param>
/// <param name="SmsSdkAppId">短信SDk 应用ID</param>
public record SaveMobileMessageSettingCommand(
    MobileMessagePlatform MobileMessagePlatform,
    string AppId,
    string AppSecret,
    List<string> SignList,
    string DefaultSign,
    List<MessageTemplate> Templates,
    string SmsSdkAppId
) : Command("修改手机短信配置");