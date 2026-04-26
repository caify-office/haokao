using HaoKao.QuestionService.Domain.QuestionModule;

namespace HaoKao.QuestionService.Domain.QuestionCollectionModule;

/// <summary>
/// 试题收藏
/// </summary>
public class QuestionCollection : AggregateRoot<Guid>,
                                  IIncludeCreatorId<Guid>,
                                  IIncludeCreateTime,
                                  IIncludeMultiTenant<Guid>,
                                  ITenantShardingTable
{
    public QuestionCollection() { }

    public QuestionCollection(Guid questionId)
    {
        QuestionId = questionId;
    }

    /// <summary>
    /// 试题Id
    /// </summary>
    public Guid QuestionId { get; set; }

    /// <summary>
    /// 创建者Id，用户Id
    /// </summary>
    public Guid CreatorId { get; set; }

    /// <summary>
    /// 收藏时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 租户Id
    /// </summary>
    public Guid TenantId { get; set; }

    /// <summary>
    /// 试题对象
    /// </summary>
    public Question Question { get; set; }
}