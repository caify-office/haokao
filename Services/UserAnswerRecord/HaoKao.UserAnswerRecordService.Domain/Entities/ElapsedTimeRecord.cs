using HaoKao.Common.Enums;

namespace HaoKao.UserAnswerRecordService.Domain.Entities;

/// <summary>
/// 记录用户做题时长
/// </summary>
[Comment("用户做题时长")]
public class ElapsedTimeRecord : AggregateRoot<Guid>,
                                 IIncludeCreatorId<Guid>,
                                 IIncludeMultiTenant<Guid>,
                                 IIncludeCreateTime,
                                 ITenantShardingTable
{
    /// <summary>
    /// 科目Id
    /// </summary>
    public Guid SubjectId { get; set; }

    /// <summary>
    /// 目标Id
    /// </summary>
    public Guid TargetId { get; set; }

    /// <summary>
    /// 产品Id
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// 类型
    /// </summary>
    public SubmitAnswerType Type { get; set; }

    /// <summary>
    /// 总题数
    /// </summary>
    public int QuestionCount { get; set; }

    /// <summary>
    /// 作答数
    /// </summary>
    public int AnswerCount { get; set; }

    /// <summary>
    /// 正确数
    /// </summary>
    public int CorrectCount { get; set; }

    /// <summary>
    /// 耗时(秒)
    /// </summary>
    public long ElapsedSeconds { get; set; }

    /// <summary>
    /// 用户Id
    /// </summary>
    public Guid CreatorId { get; set; }

    /// <summary>
    /// 做题时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 做题日期
    /// </summary>
    public DateOnly CreateDate { get; set; }

    /// <summary>
    /// 租户ID
    /// </summary>
    public Guid TenantId { get; set; }
}