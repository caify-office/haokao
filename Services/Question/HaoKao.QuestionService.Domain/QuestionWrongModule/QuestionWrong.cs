using HaoKao.QuestionService.Domain.QuestionModule;

namespace HaoKao.QuestionService.Domain.QuestionWrongModule;

/// <summary>
/// 错题集
/// </summary>
public sealed class QuestionWrong : AggregateRoot<Guid>,
                                    IIncludeCreatorId<Guid>,
                                    IIncludeMultiTenant<Guid>,
                                    ITenantShardingTable,
                                    IIncludeCreateTime
{
    /// <summary>
    /// 试题Id
    /// </summary>
    public Guid QuestionId { get; set; }

    /// <summary>
    /// 试题类型Id
    /// </summary>
    public Guid QuestionTypeId { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; }

    /// <summary>
    /// 父试题Id
    /// </summary>
    public Guid ParentQuestionId { get; set; }

    /// <summary>
    /// 父试题类型Id
    /// </summary>
    public Guid ParentQuestionTypeId { get; set; }

    /// <summary>
    /// 是否激活状态的错题, true 待消灭, false 被消灭
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// 创建者，对应的用户Id
    /// </summary>
    public Guid CreatorId { get; set; }

    /// <summary>
    /// 租户Id
    /// </summary>
    public Guid TenantId { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 试题对象
    /// </summary>
    public Question Question { get; set; }
}