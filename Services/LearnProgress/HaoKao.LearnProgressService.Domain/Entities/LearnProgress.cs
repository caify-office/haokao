using System;

namespace HaoKao.LearnProgressService.Domain.Entities;

public class LearnProgress : AggregateRoot<Guid>, IIncludeCreateTime, IIncludeMultiTenant<Guid>, ITenantShardingTable, IIncludeCreatorId<Guid>, IIncludeCreatorName
{
    /// <summary>
    /// 产品Id
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// 对应的科目Id
    /// </summary>
    public Guid SubjectId { get; set; }

    /// <summary>
    /// 章节id
    /// </summary>
    public Guid ChapterId { get; set; }

    /// <summary>
    /// 课程id
    /// </summary>
    public Guid CourseId { get; set; }

    /// <summary>
    /// 视频id
    /// </summary>
    public Guid VideoId { get; set; }

    /// <summary>
    /// 当前视频标识符,
    /// </summary>
    public string Identifier { get; set; }

    /// <summary>
    /// 本次  学习时长(单位:s)
    /// </summary>
    public float Progress { get; set; }

    /// <summary>
    /// 视频总长度(单位:s)--冗余,用于进度百分比计算
    /// </summary>
    public float TotalProgress { get; set; }

    /// <summary>
    /// 观看视频最大长度(单位:s)
    /// </summary>
    public float MaxProgress { get; set; }

    /// <summary>
    /// 创建时间(也当更新时间使用)
    /// </summary>
    public DateTime CreateTime { get; set; }

    public Guid TenantId { get; set; }

    public Guid CreatorId { get; set; }

    public string CreatorName { get; set; }

    /// <summary>
    /// 视频是否学完
    /// </summary>
    public bool IsEnd { get; set; }
}