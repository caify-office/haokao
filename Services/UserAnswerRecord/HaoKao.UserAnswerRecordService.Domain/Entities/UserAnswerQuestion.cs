using HaoKao.Common.Enums;

namespace HaoKao.UserAnswerRecordService.Domain.Entities;

/// <summary>
/// 答题记录试题表
/// </summary>
[Comment("答题记录试题表")]
public class UserAnswerQuestion : AggregateRoot<Guid>,
                                  IIncludeMultiTenant<Guid>,
                                  ITenantShardingTable,
                                  IYearShardingTable
{
    /// <summary>
    /// 答题记录Id(外键)
    /// </summary>
    public Guid UserAnswerRecordId { get; set; }

    /// <summary>
    /// 试题Id
    /// </summary>
    public Guid QuestionId { get; set; }

    /// <summary>
    /// 试题父Id
    /// </summary>
    public Guid? ParentId { get; set; }

    /// <summary>
    /// 试题类型Id
    /// </summary>
    public Guid QuestionTypeId { get; set; }

    /// <summary>
    /// 用户得分
    /// </summary>
    public decimal UserScore { get; set; }

    /// <summary>
    /// 判题结果
    /// </summary>
    public ScoringRuleType JudgeResult { get; set; }

    /// <summary>
    /// 是否标记
    /// </summary>
    public bool WhetherMark { get; set; }

    /// <summary>
    /// 租户ID
    /// </summary>
    public Guid TenantId { get; set; }

    /// <summary>
    /// 用户作答
    /// </summary>
    public List<UserQuestionOption> QuestionOptions { get; set; } = [];
}