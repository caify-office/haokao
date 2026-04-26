namespace HaoKao.WebsiteConfigurationService.Domain.Commands.WebsiteTemplate;
/// <summary>
/// 设置是否启用
/// </summary>
/// <param name="Id"></param>
/// <param name="IsEnable">是否启用</param>
public record SetWebsiteTemplateIsEnableCommand(
   Guid Id,
   bool IsEnable
) : Command("设置是否启用");