namespace HaoKao.FeedBackService.Domain.Entities;

/// <summary>
/// 答疑回复
/// </summary>
public class FeedBackReply : AggregateRoot<Guid>,
                             IIncludeCreateTime,
                             IIncludeMultiTenant<Guid>,
                             ITenantShardingTable,
                             IIncludeCreatorId<Guid>
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
    public Guid FeedBackId { get; set; }

    /// <summary>
    /// 上传的文件地址
    /// </summary>
    public string FileUrl { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    public Guid TenantId { get; set; }

    /// <summary>
    /// 创建人
    /// </summary>
    public Guid CreatorId { get; set; }

    public string CreatorName { get; set; }
}