namespace HaoKao.AgreementService.Application.ViewModels.StudentAgreement;

[AutoMapFrom(typeof(Domain.Entities.StudentAgreement))]
public record BrowseStudentAgreementViewModel : IDto
{
    /// <summary>
    /// 协议Id
    /// </summary>
    public Guid AgreementId { get; init; }

    /// <summary>
    /// 协议名称
    /// </summary>
    public string AgreementName { get; init; }

    /// <summary>
    /// 学员名称
    /// </summary>
    public string StudentName { get; init; }

    /// <summary>
    /// 身份证号
    /// </summary>
    public string IdCard { get; init; }

    /// <summary>
    /// 联系电话
    /// </summary>
    public string Contact { get; init; }

    /// <summary>
    /// 联系地址
    /// </summary>
    public string Address { get; init; }

    /// <summary>
    /// 电子邮箱
    /// </summary>
    public string Email { get; init; }

    /// <summary>
    /// 签署人Id
    /// </summary>
    public Guid CreatorId { get; init; }
}