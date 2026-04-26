namespace HaoKao.ProductService.Domain.Commands.ProductPackage;

/// <summary>
/// 禁用启用命令
/// </summary>
/// <param name="Id">主键</param>
/// <param name="Enable">启用</param>
public record SetProductPackageEnableCommand(Guid Id, bool Enable) : Command("禁用启用");

/// <summary>
/// 设置产品命令
/// </summary>
/// <param name="Id">主键</param>
/// <param name="ProductList">对应的产品列表</param>
public record SetProductPackageProductListCommand(Guid Id, List<Guid> ProductList) : Command("设置产品");

/// <summary>
/// 上架下架命令
/// </summary>
/// <param name="Id">主键</param>
/// <param name="Shelves">上架</param>
public record SetProductPackageShelvesCommand(Guid Id, bool Shelves) : Command("上架下架");