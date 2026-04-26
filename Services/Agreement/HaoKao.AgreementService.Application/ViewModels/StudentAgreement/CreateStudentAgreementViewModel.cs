using HaoKao.AgreementService.Domain.Commands;

namespace HaoKao.AgreementService.Application.ViewModels.StudentAgreement;

[AutoMapTo(typeof(CreateStudentAgreementCommand))]
public record CreateStudentAgreementViewModel : IDto
{
    /// <summary>
    /// 产品Id
    /// </summary>
    [DisplayName("产品Id")]
    [Required(ErrorMessage = "{0}不能为空")]
    public Guid ProductId { get; init; }

    /// <summary>
    /// 产品名称
    /// </summary>
    [DisplayName("产品名称")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MaxLength(50, ErrorMessage = "{0}长度不能大于{1}")]
    public string ProductName { get; init; }

    /// <summary>
    /// 协议Id
    /// </summary>
    [DisplayName("协议Id")]
    [Required(ErrorMessage = "{0}不能为空")]
    public Guid AgreementId { get; init; }

    /// <summary>
    /// 协议名称
    /// </summary>
    [DisplayName("协议名称")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MaxLength(50, ErrorMessage = "{0}长度不能大于{1}")]
    public string AgreementName { get; init; }

    /// <summary>
    /// 学员名称
    /// </summary>
    [DisplayName("学员名称")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MaxLength(50, ErrorMessage = "{0}长度不能大于{1}")]
    public string StudentName { get; init; }

    /// <summary>
    /// 身份证号
    /// </summary>
    [DisplayName("身份证号")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MaxLength(18, ErrorMessage = "{0}长度不能大于{1}")]
    public string IdCard { get; init; }

    /// <summary>
    /// 联系电话
    /// </summary>
    [DisplayName("联系电话")]
    [MaxLength(20, ErrorMessage = "{0}长度不能大于{1}")]
    public string Contact { get; init; } = "";

    /// <summary>
    /// 联系地址
    /// </summary>
    [DisplayName("联系地址")]
    [MaxLength(50, ErrorMessage = "{0}长度不能大于{1}")]
    public string Address { get; init; } = "";

    /// <summary>
    /// 电子邮箱
    /// </summary>
    [DisplayName("电子邮箱")]
    [MaxLength(50, ErrorMessage = "{0}长度不能大于{1}")]
    public string Email { get; init; } = "";
}