using System;

namespace HaoKao.LearnProgressService.Domain.Entities;

public class DailyStudyDuration : AggregateRoot<Guid>, IIncludeMultiTenant<Guid>, ITenantShardingTable, IIncludeCreatorId<Guid>, IIncludeCreatorName
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
    /// 今日视频学习时长
    /// </summary>
    public decimal DailyVideoStudyDuration { get; set; }

    /// <summary>
    /// 学习时间
    /// </summary>
    public DateOnly LearnTime { get; set; }

    public Guid TenantId { get; set; }

    public Guid CreatorId { get; set; }

    public string CreatorName { get; set; }
}