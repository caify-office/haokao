namespace HaoKao.ProductService.Application.ViewModels.StudentPermission;

[AutoMapTo(typeof(Domain.Entities.StudentPermission))]
public class CreateStudentPermissionViewModel : IDto
{
    /// <summary>
    /// 学员昵称(即用户昵称)
    /// </summary>
    [DisplayName("学员昵称(即用户昵称)")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MaxLength(50, ErrorMessage = "{0}长度不能大于{1}")]
    public string StudentName { get; set; }

    /// <summary>
    /// 学员ID（即用户ID）
    /// </summary>
    [DisplayName("学员ID（即用户ID）")]
    [Required(ErrorMessage = "{0}不能为空")]
    public Guid StudentId { get; set; }

    /// <summary>
    /// 对应的订单号
    /// </summary>
    [DisplayName("对应的订单号")]
    public string OrderNumber { get; set; }

    /// <summary>
    /// 产品Id
    /// </summary>
    [DisplayName("产品Id")]
    [Required(ErrorMessage = "{0}不能为空")]
    public Guid ProductId { get; set; }

    /// <summary>
    /// 产品名称
    /// </summary>
    [DisplayName("产品名称")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MinLength(2, ErrorMessage = "{0}长度不能小于{1}")]
    [MaxLength(150, ErrorMessage = "{0}长度不能大于{1}")]
    public string ProductName { get; set; }

    /// <summary>
    /// 到期时间
    /// </summary>
    [DisplayName("到期时间")]
    [Required(ErrorMessage = "{0}不能为空")]
    public DateTime ExpiryTime { get; set; }

    /// <summary>
    /// 启用/禁用
    /// </summary>
    [DisplayName("启用/禁用")]
    [Required(ErrorMessage = "{0}不能为空")]
    public bool Enable { get; set; }
}