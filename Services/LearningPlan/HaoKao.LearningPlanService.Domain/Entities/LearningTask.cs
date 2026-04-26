using HaoKao.LearningPlanService.Domain.Enumerations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace HaoKao.LearningPlanService.Domain.Entities;

/// <summary>
/// 学习任务主类
/// </summary>
public class LearningTask : AggregateRoot<Guid>, IIncludeCreatorId<Guid>, IIncludeCreatorName, IIncludeCreateTime, IIncludeUpdateTime, IIncludeMultiTenant<Guid>, ITenantShardingTable
{
    /// <summary>
    /// 任务名称
    /// </summary>
    public string TaskName { get; set; }

    /// <summary>
    /// 计划开始该任务的时间点
    /// </summary>
    public DateOnly ScheduledTime { get; set; }

    /// <summary>
    /// 完成该任务预计需要的时长（秒），支持小数表示
    /// </summary>
    public decimal DurationSeconds { get; set; }

    /// <summary>
    /// 任务类型
    /// </summary>
    public TaskType TaskType { get; set; }

    /// <summary>
    /// 任务标识（用于重新制作学习计划时判定当前任务内容是否已是过期任务内容）
    /// </summary>
    public string TaskIdentity { get; set; }

    /// <summary>
    /// 任务内容（存视频任务，章节任务任务，试卷任务详情）
    /// </summary>

    public string TaskContent { get; set; }

    /// <summary>
    /// 关联的学习计划ID
    /// </summary>
    public Guid LearningPlanId { get; set; }

    /// <summary>
    /// 导航属性，指向关联的学习计划
    /// </summary>
    [NotMapped]
    [JsonIgnore]
    [Newtonsoft.Json.JsonIgnore]
    public LearningPlan LearningPlan { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; }

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