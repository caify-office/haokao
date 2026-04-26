using HaoKao.Common.Enums;

namespace HaoKao.UserAnswerRecordService.Application.Modules.UserAnswerRecordModule.ViewModels;

/// <summary>
/// 练习状况分析结果类
/// </summary>
public record PracticeSituationAnalysisViewModel : IDto
{
    /// <summary>
    /// 已答
    /// </summary>
    public int AnswerCount { get; set; }

    /// <summary>
    /// 答对
    /// </summary>
    public int CorrectCount { get; set; }

    /// <summary>
    /// 错误
    /// </summary>
    public int WrongCount { get; set; }

    /// <summary>
    /// 少选
    /// </summary>
    public int MissedCount { get; set; }

    /// <summary>
    /// 总答题数量
    /// </summary>
    public int TotalAnswerCount { get; set; }

    /// <summary>
    /// 总答对题数
    /// </summary>
    public int TotalCorrectCount { get; set; }

    /// <summary>
    /// 不同题型的准确率
    /// </summary>
    public Dictionary<Guid, double> QuestionTypeAccuracy { get; set; } = [];

    /// <summary>
    /// 同期学员不同题型的准确率
    /// </summary>
    public Dictionary<Guid, double> AverageQuestionTypeAccuracy { get; set; } = [];

    /// <summary>
    /// 我的准确率
    /// </summary>
    public double MyAccuracy { get; set; }

    /// <summary>
    /// 同期学员的准确率
    /// </summary>
    public double AverageAccuracy { get; set; }

    /// <summary>
    /// 我的准确率百分比排名
    /// </summary>
    public double MyAccuracyPercentileRank { get; set; }

    /// <summary>
    /// 我的练习进度
    /// </summary>
    public double MyProgress { get; set; }

    /// <summary>
    /// 同期学员的练习进度
    /// </summary>
    public double AverageProgress { get; set; }

    /// <summary>
    /// 刷新时间
    /// </summary>
    public DateTime? RefreshTime { get; set; }

    /// <summary>
    /// 是否可以刷新
    /// </summary>
    public bool Refreshable { get; set; } = true;

    /// <summary>
    /// 试题总数
    /// </summary>
    public int QuestionCount { get; set; }

    /// <summary>
    /// 同期学员平均作答数
    /// </summary>
    public int AverageAnswerCount { get; set; }
}

/// <summary>
/// 做题能力分析结果类
/// </summary>
public record PracticeAbilityAnalysisViewModel
{
    /// <summary>
    /// 同期学员的能力
    /// </summary>
    public Dictionary<string, double> AverageAbility { get; init; }

    /// <summary>
    /// 我的能力
    /// </summary>
    public Dictionary<string, double> MyAbility { get; init; }

    /// <summary>
    /// 刷新时间
    /// </summary>
    public DateTime? RefreshTime { get; set; }

    /// <summary>
    /// 是否可以刷新
    /// </summary>
    public bool Refreshable { get; set; } = true;
}

/// <summary>
/// 练习统计分析模型
/// </summary>
public record PracticeStaticsAnalysisModel
{
    /// <summary>
    /// 用户Id
    /// </summary>
    public Guid UserId { get; init; }

    /// <summary>
    /// 试题Id
    /// </summary>
    public Guid QuestionId { get; init; }

    /// <summary>
    /// 题型Id
    /// </summary>
    public Guid QuestionTypeId { get; init; }

    /// <summary>
    /// 父题目Id
    /// </summary>
    public Guid? ParentId { get; init; }

    /// <summary>
    /// 判题结果
    /// </summary>
    public ScoringRuleType JudgeResult { get; init; }

    /// <summary>
    /// 作答时间
    /// </summary>
    public DateTime CreateTime { get; init; }

    /// <summary>
    /// 能力维度
    /// </summary>
    public string[] Ability { get; init; } = [];
}

public record BuildWorkItemModel
{
    /// <summary>
    /// 科目Id
    /// </summary>
    public Guid SubjectId { get; init; }

    /// <summary>
    /// 用户Id
    /// </summary>
    public Guid UserId { get; init; }

    /// <summary>
    /// 租户Id
    /// </summary>
    public string TenantId { get; init; }

    /// <summary>
    /// 刷新缓存标识
    /// </summary>
    public CacheKey RefreshCacheKey { get; init; }

    /// <summary>
    /// 结果缓存标识
    /// </summary>
    public CacheKey ResultCacheKey { get; init; }
}

/// <summary>
/// 做题能力存储过程结果模型
/// </summary>
public record PracticeAbilitySpModel
{
    /// <summary>
    /// 计算能力(作答次数)
    /// </summary>
    public int Numeracy { get; init; }

    /// <summary>
    /// 计算能力(答对的次数)
    /// </summary>
    public int CorrectNumeracy { get; set; }

    /// <summary>
    /// 判断能力(作答次数)
    /// </summary>
    public int Judgment { get; init; }

    /// <summary>
    /// 判断能力(答对的次数)
    /// </summary>
    public int CorrectJudgment { get; set; }

    /// <summary>
    /// 分析能力(作答次数)
    /// </summary>
    public int Analytical { get; init; }

    /// <summary>
    /// 分析能力(答对的次数)
    /// </summary>
    public int CorrectAnalytical { get; set; }

    /// <summary>
    /// 理解能力(作答次数)
    /// </summary>
    public int Understanding { get; init; }

    /// <summary>
    /// 理解能力(答对的次数)
    /// </summary>
    public int CorrectUnderstanding { get; set; }

    /// <summary>
    /// 记忆能力(作答次数)
    /// </summary>
    public int Memory { get; init; }

    /// <summary>
    /// 记忆能力(答对的次数)
    /// </summary>
    public int CorrectMemory { get; set; }

    /// <summary>
    /// 用户Id
    /// </summary>
    public Guid CreatorId { get; init; }
}

/// <summary>
/// 做题速度模型
/// </summary>
public record PracticeSpeedModel
{
    /// <summary>
    /// 答题耗时
    /// </summary>
    public long ElapsedTime { get; init; }

    /// <summary>
    /// 答题数量
    /// </summary>
    public int AnswerCount { get; init; }

    /// <summary>
    /// 用户Id
    /// </summary>
    public Guid CreatorId { get; init; }
}