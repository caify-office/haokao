namespace HaoKao.WebsiteConfigurationService.Domain.Commands.TemplateStyle;
/// <summary>
/// 删除模板命令
/// </summary>
/// <param name="Id">主键</param>
public record DeleteTemplateStyleCommand(
    Guid Id
) : Command("删除模板");