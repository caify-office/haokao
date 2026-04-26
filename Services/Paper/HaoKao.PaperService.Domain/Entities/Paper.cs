using HaoKao.PaperService.Domain.Enumerations;

namespace HaoKao.PaperService.Domain.Entities;

/// <summary>
/// 试卷
/// </summary>
public class Paper : AggregateRoot<Guid>,
                     IIncludeCreateTime,
                     IIncludeMultiTenant<Guid>,
                     ITenantShardingTable
{
    /// <summary>
    /// 试卷名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 科目id
    /// </summary>
    public Guid SubjectId { get; set; }

    /// <summary>
    /// 科目名称
    /// </summary>
    public string SubjectName { get; set; }

    /// <summary>
    /// 所属分类
    /// </summary>
    public Guid CategoryId { get; set; }

    /// <summary>
    /// 分类名称
    /// </summary>
    public string CategoryName { get; set; }

    /// <summary>
    /// 考试时长
    /// </summary>
    public int Time { get; set; }

    /// <summary>
    /// 及格分数
    /// </summary>
    public decimal Score { get; set; }

    /// <summary>
    /// 总题目数
    /// </summary>
    public int QuestionCount { get; set; }

    /// <summary>
    /// 总分数
    /// </summary>
    public decimal TotalScore { get; set; }

    /// <summary>
    /// 是否限免
    /// </summary>
    public FreeEnum IsFree { get; set; }

    /// <summary>
    /// 发布状态
    /// </summary>
    public StateEnum State { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 租户Id
    /// </summary>
    public Guid TenantId { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; }

    /// <summary>
    /// 试卷结构json
    /// </summary>
    public string StructJson { get; set; }

    /// <summary>
    /// 年份
    /// </summary>
    public int Year { get; set; }
}