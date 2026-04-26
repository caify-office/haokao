using HaoKao.Common.Enums;

namespace HaoKao.UserAnswerRecordService.Domain.Entities;

/// <summary>
/// 日期相关答题记录
/// </summary>
[Comment("日期相关作答记录")]
public class DateAnswerRecord : AggregateRoot<Guid>,
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
    /// 类型
    /// </summary>
    public SubmitAnswerType Type { get; set; }

    /// <summary>
    /// 日期
    /// </summary>
    public DateOnly Date { get; set; }

    /// <summary>
    /// 用户Id
    /// </summary>
    public Guid CreatorId { get; set; }

    /// <summary>
    /// 答题时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 作答记录Id
    /// </summary>
    public Guid AnswerRecordId { get; set; }

    /// <summary>
    /// 租户Id
    /// </summary>
    public Guid TenantId { get; set; }

    /// <summary>
    /// 答题记录
    /// </summary>
    public AnswerRecord AnswerRecord { get; set; }
}