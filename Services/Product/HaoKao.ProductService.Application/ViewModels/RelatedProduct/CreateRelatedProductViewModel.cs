using HaoKao.ProductService.Domain.Commands.RelatedProduct;

namespace HaoKao.ProductService.Application.ViewModels.RelatedProduct;


[AutoMapTo(typeof(CreateRelatedProductModel))]
public class CreateRelatedProductViewModel : IDto
{
    /// <summary>
    /// 产品id
    /// </summary>
    [DisplayName("产品id")]
    [Required(ErrorMessage = "{0}不能为空")]
    public Guid ProductId{ get; set; }

    /// <summary>
    /// 产品名称
    /// </summary>
    [DisplayName("产品名称")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MinLength(2, ErrorMessage = "{0}长度不能小于{1}")]
    [MaxLength(50, ErrorMessage = "{0}长度不能大于{1}")]
    public string ProductName{ get; set; }

    /// <summary>
    /// 关联对象产品id
    /// </summary>
    [DisplayName("关联对象产品id")]
    [Required(ErrorMessage = "{0}不能为空")]
    public Guid RelatedTargetProductId{ get; set; }

    /// <summary>
    /// 关联对象产品名称
    /// </summary>
    [DisplayName("关联对象产品名称")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MinLength(2, ErrorMessage = "{0}长度不能小于{1}")]
    [MaxLength(50, ErrorMessage = "{0}长度不能大于{1}")]
    public string RelatedTargetProducName{ get; set; }
}