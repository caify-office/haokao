namespace HaoKao.WebsiteConfigurationService.Domain.Commands.WebsiteTemplate;
/// <summary>
/// 删除模板命令
/// </summary>
/// <param name="Id">主键</param>
public record DeleteWebsiteTemplateCommand(
    Guid Id
) : Command("删除模板");