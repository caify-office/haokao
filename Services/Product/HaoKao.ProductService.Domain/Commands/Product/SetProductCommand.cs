namespace HaoKao.ProductService.Domain.Commands.Product;

/// <summary>
/// 开启关闭答疑
/// </summary>
/// <param name="Ids">主键</param>
/// <param name="Answering">答疑</param>
public record SetProductAnsweringCommand(IEnumerable<Guid> Ids, bool Answering) : Command("开启关闭答疑");

/// <summary>
/// 修改产品禁用启用
/// </summary>
/// <param name="Ids"></param>
/// <param name="Enable">禁用启用</param>
public record SetProductEnableCommand(ICollection<Guid> Ids, bool Enable) : Command("修改产品禁用启用");

/// <summary>
/// 修改产品上下架
/// </summary>
/// <param name="Ids"></param>
/// <param name="IsShelves">产品上下架</param>
public record SetProductShelvesCommand(ICollection<Guid> Ids, bool IsShelves) : Command("修改产品上下架");