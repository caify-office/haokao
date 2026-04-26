namespace HaoKao.LearningPlanService.Domain.Entities;

/// <summary>
/// 学习计划主类，用于组织和管理一系列学习任务
/// </summary>
public class LearningPlan : AggregateRoot<Guid>, IIncludeCreatorId<Guid>, IIncludeCreatorName, IIncludeCreateTime, IIncludeUpdateTime, IIncludeMultiTenant<Guid>, ITenantShardingTable
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
    /// 对应科目名称
    /// </summary>
    public string SubjectName { get; set; }

    /// <summary>
    /// 学习计划结束日期
    /// </summary>
    public DateOnly EndDate { get; set; }

    /// <summary>
    /// 该学习计划包含的所有任务
    /// </summary>
    public virtual ICollection<LearningTask> LearningTasks { get; set; } = [];

    /// <summary>
    /// 每周学习时长配置(分钟)
    /// </summary>
    public ICollection<int> DayLearningTimes { get; set; } = [];

    /// <summary>
    /// 需要提醒
    /// </summary>
    public bool NeedReminder { get; set; }

    /// <summary>
    /// 提醒时间(小时)
    /// </summary>
    public int ReminderHours { get; set; }

    /// <summary>
    /// 提醒时间(分钟)
    /// </summary>
    public int ReminderMinutes { get; set; }

    /// <summary>
    /// 提醒手机号
    /// </summary>
    public string ReminderPhone { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 修改时间
    /// </summary>
    public DateTime UpdateTime { get; set; }

    /// <summary>
    /// 租户Id
    /// </summary>
    public Guid TenantId { get; set; }

    /// <summary>
    /// 创建者Id
    /// </summary>
    public Guid CreatorId { get; set; }

    /// <summary>
    /// 创建者名称
    /// </summary>
    public string CreatorName { get; set; }
}