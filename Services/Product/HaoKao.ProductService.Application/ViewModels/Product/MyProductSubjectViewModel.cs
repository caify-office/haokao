namespace HaoKao.ProductService.Application.ViewModels.Product;

[AutoMapFrom(typeof(Domain.Entities.ProductPermission))]
[AutoMapTo(typeof(Domain.Entities.ProductPermission))]
public class MyProductSubjectViewModel : IDto
{
    /// <summary>
    /// 对应的科目Id
    /// </summary>
    public Guid SubjectId { get; set; }

    /// <summary>
    /// 对应科目名称
    /// </summary>
    public string SubjectName { get; set; }
}