namespace HaoKao.ProductService.Application.ViewModels.RelatedProduct;


[AutoMapFrom(typeof(Domain.Entities.RelatedProduct))]
public class BrowseRelatedProductViewModel : IDto
{
    public Guid Id { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 产品id
    /// </summary>
    public Guid ProductId{ get; set; }

    /// <summary>
    /// 产品名称
    /// </summary>
    public string ProductName{ get; set; }

    /// <summary>
    /// 关联对象产品id
    /// </summary>
    public Guid RelatedTargetProductId{ get; set; }

    /// <summary>
    /// 关联对象产品名称
    /// </summary>
    public string RelatedTargetProducName{ get; set; }
}
