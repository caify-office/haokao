namespace HaoKao.ProductService.Application.ViewModels.SupervisorClass;


[AutoMapTo(typeof(Domain.Commands.SupervisorClass.CreateSupervisorClassCommand))]
public class CreateSupervisorClassViewModel : IDto
{

    /// <summary>
    /// 班级名称
    /// </summary>
    [DisplayName("班级名称")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MinLength(2, ErrorMessage = "{0}长度不能小于{1}")]
    [MaxLength(100, ErrorMessage = "{0}长度不能大于{1}")]
    public string Name { get; set; }

    /// <summary>
    /// 年份
    /// </summary>
    [DisplayName("年份")]
    [Required(ErrorMessage = "{0}不能为空")]
    public int Year { get; set; }

    /// <summary>
    /// 产品包id
    /// </summary>
    [DisplayName("产品包id")]
    [Required(ErrorMessage = "{0}不能为空")]
    public Guid ProductPackageId { get; set; }

    /// <summary>
    /// 产品包名称
    /// </summary>
    [DisplayName("产品包名称")]
    public string ProductPackageName { get; set; }

    /// <summary>
    /// 产品id
    /// </summary>
    [DisplayName("产品id")]
    [Required(ErrorMessage = "{0}不能为空")]
    public Guid ProductId { get; set; }

    /// <summary>
    /// 产品名称
    /// </summary>
    [DisplayName("产品名称")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MinLength(2, ErrorMessage = "{0}长度不能小于{1}")]
    [MaxLength(100, ErrorMessage = "{0}长度不能大于{1}")]
    public string ProductName { get; set; }

    /// <summary>
    /// 营销人员Id
    /// </summary>
    [DisplayName("营销人员Id")]
    [Required(ErrorMessage = "{0}不能为空")]
    public Guid SalespersonId { get; set; }

    /// <summary>
    /// 营销人员名称
    /// </summary>
    [DisplayName("营销人员名称")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MinLength(2, ErrorMessage = "{0}长度不能小于{1}")]
    [MaxLength(50, ErrorMessage = "{0}长度不能大于{1}")]
    public string SalespersonName { get; set; }

}