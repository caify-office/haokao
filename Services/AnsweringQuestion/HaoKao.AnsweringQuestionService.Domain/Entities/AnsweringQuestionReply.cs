using System;

namespace HaoKao.AnsweringQuestionService.Domain.Entities;

/// <summary>
/// 答疑回复
/// </summary>
public class AnsweringQuestionReply : AggregateRoot<Guid>, IIncludeCreateTime, IIncludeMultiTenant<Guid>, ITenantShardingTable, IIncludeCreatorId<Guid>
{
    /// <summary>
    /// 答疑回复内容
    /// </summary>

    public string ReplyContent { get; set; }

    /// <summary>
    /// 回复人用户名
    /// </summary>
    public string ReplyUserName { get; set; }

    /// <summary>
    /// 关联的题目id
    /// </summary>
    public Guid AnsweringQuestionId { get; set; }
    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    public Guid TenantId { get; set; }

    /// <summary>
    /// 创建人
    /// </summary>
    public Guid CreatorId { get; set; }


}