using HaoKao.FeedBackService.Domain.Enums;
using System.Collections.Generic;

namespace HaoKao.FeedBackService.Domain.Entities;

public sealed class FeedBack : AggregateRoot<Guid>,
                               IIncludeCreateTime,
                               IIncludeMultiTenant<Guid>,
                               ITenantShardingTable,
                               IIncludeCreatorId<Guid>,
                               IIncludeCreatorName
{
    /// <summary>
    /// 反馈类型
    /// </summary>
    public TypeEnum Type { get; set; }

    /// <summary>
    /// 反馈来源
    /// </summary>
    public SourceTypeEnum SourceType { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    public StatusEnum Status { get; set; }

    /// <summary>
    /// 联系方式
    /// </summary>
    public string Contract { get; set; }

    /// <summary>
    /// 描述
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// 图片
    /// </summary>
    public string FileUrls { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 创建人
    /// </summary>
    public Guid CreatorId { get; set; }

    public string CreatorName { get; set; }

    public Guid TenantId { get; set; }

    public Guid? ParentId { get; set; }

    /// <summary>
    ///回复列表
    /// </summary>
    public List<FeedBackReply> FeedBackReplies { get; set; }

    /// <summary>
    ///追问
    /// </summary>
    [ForeignKey("ParentId")]
    public List<FeedBack> ChildQuestion { get; set; }
}