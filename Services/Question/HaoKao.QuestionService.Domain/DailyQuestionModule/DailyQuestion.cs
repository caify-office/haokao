namespace HaoKao.QuestionService.Domain.DailyQuestionModule;

/// <summary>
/// 每日一题
/// </summary>
[Comment("每日一题")]
public sealed class DailyQuestion : AggregateRoot<Guid>,
                                    IIncludeMultiTenant<Guid>,
                                    ITenantShardingTable
{
    /// <summary>
    /// 科目Id
    /// </summary>
    public Guid SubjectId { get; set; }

    /// <summary>
    /// 试题Id
    /// </summary>
    public Guid QuestionId { get; set; }

    /// <summary>
    /// 创建日期
    /// </summary>
    public DateTime CreateDate { get; set; }

    /// <summary>
    /// 租户Id
    /// </summary>
    public Guid TenantId { get; set; }
}