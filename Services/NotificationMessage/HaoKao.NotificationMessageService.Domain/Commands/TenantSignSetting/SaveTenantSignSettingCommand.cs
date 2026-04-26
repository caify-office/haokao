namespace HaoKao.NotificationMessageService.Domain.Commands.TenantSignSetting;
/// <summary>
/// 修改当前考试短信签名
/// </summary>
/// <param name="Sign">短信签名</param>
public record SaveTenantSignSettingCommand(string Sign) : Command("修改当前考试短信签名");