namespace HaoKao.UserAnswerRecordService.Domain.Entities;

/// <summary>
/// 用户答题的选项或内容
/// </summary>
[Comment("用户答题的选项或内容")]
public class UserQuestionOption : AggregateRoot<Guid>,
                                  IIncludeMultiTenant<Guid>,
                                  ITenantShardingTable,
                                  IYearShardingTable
{
    /// <summary>
    /// 选项Id
    /// </summary>
    public Guid OptionId { get; set; }

    /// <summary>
    /// 回答内容
    /// </summary>
    public string OptionContent { get; set; }

    /// <summary>
    /// 租户Id
    /// </summary>
    public Guid TenantId { get; set; }
}