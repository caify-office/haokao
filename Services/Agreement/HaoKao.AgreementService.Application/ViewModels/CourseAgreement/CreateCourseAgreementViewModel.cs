using HaoKao.AgreementService.Domain.Commands;
using HaoKao.AgreementService.Domain.Entities;

namespace HaoKao.AgreementService.Application.ViewModels.CourseAgreement;

[AutoMapTo(typeof(CreateCourseAgreementCommand))]
public record CreateCourseAgreementViewModel : IDto
{
    /// <summary>
    /// 协议名称
    /// </summary>
    [DisplayName("协议名称")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MaxLength(50, ErrorMessage = "{0}长度不能大于{1}")]
    public string Name { get; init; }

    /// <summary>
    /// 协议内容
    /// </summary>
    [DisplayName("协议内容")]
    [Required(ErrorMessage = "{0}不能为空")]
    public string Content { get; init; }

    /// <summary>
    /// 续读次数
    /// </summary>
    [DisplayName("续读次数")]
    [Required(ErrorMessage = "{0}不能为空")]
    public int Continuation { get; init; }

    /// <summary>
    /// 协议类型
    /// </summary>
    [DisplayName("协议类型")]
    [Required(ErrorMessage = "{0}不能为空")]
    public AgreementType AgreementType { get; init; }
}