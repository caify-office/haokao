namespace HaoKao.ProductService.Domain.Commands.ProductPackage;

/// <summary>
/// 删除产品包命令
/// </summary>
/// <param name="Id">主键</param>
public record DeleteProductPackageCommand(Guid Id) : Command("删除产品包");