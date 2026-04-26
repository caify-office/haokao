namespace HaoKao.ProductService.Domain.Commands.RelatedProduct;

/// <summary>
/// 批量创建关联产品命令
/// </summary>
/// <param name="Models">产品id</param>
public record CreateRelatedProductCommand(IList<CreateRelatedProductModel> Models) : Command("批量创建关联产品");

/// <summary>
/// 创建关联产品模型
/// </summary>
/// <param name="ProductId">产品id</param>
/// <param name="ProductName">产品名称</param>
/// <param name="RelatedTargetProductId">关联对象产品id</param>
/// <param name="RelatedTargetProducName">关联对象产品名称</param>
public record CreateRelatedProductModel(
    Guid ProductId,
    string ProductName,
    Guid RelatedTargetProductId,
    string RelatedTargetProducName
);

/// <summary>
/// 删除关联产品命令
/// </summary>
/// <param name="Ids">主键</param>
public record DeleteRelatedProductCommand(Guid[] Ids) : Command("删除关联产品");