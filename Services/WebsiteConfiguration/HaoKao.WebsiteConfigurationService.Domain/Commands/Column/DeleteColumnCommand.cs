namespace HaoKao.WebsiteConfigurationService.Domain.Commands.Column;
/// <summary>
/// 删除栏目命令
/// </summary>
/// <param name="Id">主键</param>
public record DeleteColumnCommand(
    Guid Id
) : Command("删除栏目");