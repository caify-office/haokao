namespace HaoKao.ProductService.Domain.Commands.Product;

/// <summary>
/// 删除产品
/// </summary>
/// <param name="Ids"></param>
public record DeleteProductCommand(ICollection<Guid> Ids) : Command("删除产品");