namespace HaoKao.ProductService.Application.ViewModels.ProductPackage;

[AutoMapTo(typeof(Domain.Entities.ProductPackage))]
public class SetProductPackageProductListViewModel : IDto
{
    /// <summary>
    /// 对应的产品列表
    /// </summary>
    [DisplayName("对应的产品列表")]
    public List<Guid> ProductList { get; set; } = [];
}