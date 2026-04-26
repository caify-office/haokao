using HaoKao.AgreementService.Domain.Commands;

namespace HaoKao.AgreementService.Application.ViewModels.StudentAgreement;

[AutoMapTo(typeof(UpdateStudentAgreementCommand))]
public record UpdateStudentAgreementViewModel : IDto
{
    /// <summary>
    /// 主键Id
    /// </summary>
    public Guid Id { get; set; }

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