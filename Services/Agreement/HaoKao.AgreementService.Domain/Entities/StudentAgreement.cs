namespace HaoKao.AgreementService.Domain.Entities;

/// <summary>
/// 学员协议
/// </summary>
[Comment("学员协议")]
public class StudentAgreement : AggregateRoot<Guid>,
                                ITenantShardingTable,
                                IIncludeMultiTenant<Guid>,
                                IIncludeCreateTime,
                                IIncludeUpdateTime,
                                IIncludeCreatorId<Guid>
{
    /// <summary>
    /// 产品Id
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// 产品名称
    /// </summary>
    public string ProductName { get; set; }

    /// <summary>
    /// 协议Id
    /// </summary>
    public Guid AgreementId { get; set; }

    /// <summary>
    /// 协议名称
    /// </summary>
    public string AgreementName { get; set; }

    /// <summary>
    /// 学员名称
    /// </summary>
    public string StudentName { get; set; }

    /// <summary>
    /// 身份证号
    /// </summary>
    public string IdCard { get; set; }

    /// <summary>
    /// 联系电话
    /// </summary>
    public string Contact { get; set; }

    /// <summary>
    /// 联系地址
    /// </summary>
    public string Address { get; set; }

    /// <summary>
    /// 电子邮箱
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// 租户Id
    /// </summary>
    public Guid TenantId { get; set; }

    /// <summary>
    /// 签署时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 修改时间
    /// </summary>
    public DateTime UpdateTime { get; set; }

    /// <summary>
    /// 签署人Id
    /// </summary>
    public Guid CreatorId { get; set; }
}