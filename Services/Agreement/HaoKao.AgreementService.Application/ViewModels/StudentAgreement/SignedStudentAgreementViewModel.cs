namespace HaoKao.AgreementService.Application.ViewModels.StudentAgreement;

[AutoMapFrom(typeof(Domain.Entities.StudentAgreement))]
public record SignedStudentAgreementViewModel : IDto
{
    /// <summary>
    /// 产品名称
    /// </summary>
    public string ProductName { get; init; }

    /// <summary>
    /// 协议名称
    /// </summary>
    public string AgreementName { get; init; }

    /// <summary>
    /// 协议内容
    /// </summary>
    public string AgreementContent { get; set; }

    /// <summary>
    /// 姓名
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
}

public record QueryMyAgreementViewModel : IDto
{
    /// <summary>
    /// 产品Id
    /// </summary>
    public Guid ProductId { get; init; }

    /// <summary>
    /// 协议Id
    /// </summary>
    public Guid AgreementId { get; init; }
}

[AutoMapFrom(typeof(Domain.Entities.StudentAgreement))]
public record MyAgreementListViewModel : IDto
{
    /// <summary>
    /// 主键Id
    /// </summary>
    public Guid? Id { get; init; }

    /// <summary>
    /// 产品Id
    /// </summary>
    public Guid ProductId { get; init; }

    /// <summary>
    /// 协议Id
    /// </summary>
    public Guid AgreementId { get; init; }

    /// <summary>
    /// 协议名称
    /// </summary>
    public string AgreementName { get; init; }

    /// <summary>
    /// 签署日期
    /// </summary>
    public DateTime? CreateTime { get; init; }
}