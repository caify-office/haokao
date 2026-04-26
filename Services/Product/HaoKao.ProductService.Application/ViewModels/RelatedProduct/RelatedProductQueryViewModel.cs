using HaoKao.ProductService.Domain.Queries;

namespace HaoKao.ProductService.Application.ViewModels.RelatedProduct;


[AutoMapFrom(typeof(RelatedProductQuery))]
[AutoMapTo(typeof(RelatedProductQuery))]
public class RelatedProductQueryViewModel: QueryDtoBase<RelatedProductQueryListViewModel>
{
    /// <summary>
    /// 产品Id
    /// </summary>
    public Guid? ProductId { get; set; }

    /// <summary>
    /// 关联对象产品id
    /// </summary>
    public Guid? RelatedTargetProductId { get; set; }
    /// <summary>
    /// 产品名称
    /// </summary>
    public string ProductName { get; set; }


    /// <summary>
    /// 关联对象产品名称
    /// </summary>
    public string RelatedTargetProducName { get; set; }
}

[AutoMapFrom(typeof(Domain.Entities.RelatedProduct))]
[AutoMapTo(typeof(Domain.Entities.RelatedProduct))]
public class RelatedProductQueryListViewModel : IDto
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

    /// <summary>
    /// 创建者
    /// </summary>
    public string CreatorName { get; set; }
}