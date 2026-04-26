using HaoKao.AgreementService.Domain.Entities;

namespace HaoKao.AgreementService.Application.ViewModels.CourseAgreement;

[AutoMapFrom(typeof(Domain.Entities.CourseAgreement))]
[AutoMapTo(typeof(Domain.Entities.CourseAgreement))]
public record BrowseCourseAgreementViewModel : IDto
{
    /// <summary>
    /// 主键
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// 协议名称
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// 协议内容
    /// </summary>
    public string Content { get; init; }

    /// <summary>
    /// 续读次数
    /// </summary>
    public int Continuation { get; init; }

    /// <summary>
    /// 协议类型
    /// </summary>
    public AgreementType AgreementType { get; init; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; init; }
}