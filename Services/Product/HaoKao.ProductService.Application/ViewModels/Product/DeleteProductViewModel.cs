namespace HaoKao.ProductService.Application.ViewModels.Product;

public class DeleteProductViewModel
{
    public ICollection<Guid> Ids { get; set; } = new List<Guid>();
}