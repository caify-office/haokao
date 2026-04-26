namespace HaoKao.WebsiteConfigurationService.Domain.Commands.WebsiteTemplate;
/// <summary>
/// 设置默认
/// </summary>
/// <param name="Id"></param>
/// <param name="isDefault"></param>
public record SetWebsiteTemplateDefaultCommand(
   Guid Id, bool isDefault
) : Command("设置默认");