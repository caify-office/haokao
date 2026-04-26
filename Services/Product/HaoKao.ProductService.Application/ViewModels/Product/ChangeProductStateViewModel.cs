namespace HaoKao.ProductService.Application.ViewModels.Product;

public class ChangeProductStateViewModel
{
    public ICollection<Guid> Ids { get; set; } = new List<Guid>();
}