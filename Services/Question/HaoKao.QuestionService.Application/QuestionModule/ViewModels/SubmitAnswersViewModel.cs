using HaoKao.Common.Enums;
using HaoKao.QuestionService.Application.QuestionHandlers;
using HaoKao.QuestionService.Application.QuestionHandlers.AnswerArea;

namespace HaoKao.QuestionService.Application.QuestionModule.ViewModels;

public class SubmitAnswersViewModel
{
    /// <summary>
    /// 科目Id
    /// </summary>
    public Guid SubjectId { get; set; }

    /// <summary>
    /// 分类Id
    /// </summary>
    public Guid CategoryId { get; set; }

    /// <summary>
    /// 试卷Id
    /// </summary>
    public Guid? PaperId { get; set; }

    /// <summary>
    /// 智辅产品Id
    /// </summary>
    public Guid? ProductId { get; set; }

    /// <summary>
    /// 标识符名称
    /// </summary>
    public string RecordIdentifierName { get; set; }

    /// <summary>
    /// 考试频率
    /// </summary>
    public int? ExamFrequency { get; set; }

    /// <summary>
    /// 答题类型
    /// </summary>
    public SubmitAnswerType AnswerType { get; set; }

    /// <summary>
    /// 耗时
    /// </summary>
    public long ElapsedTime { get; set; }

    /// <summary>
    /// 及格分数
    /// </summary>
    public decimal PassingScore { get; set; }

    /// <summary>
    /// 总分
    /// </summary>
    public decimal TotalScore { get; set; }

    /// <summary>
    /// 总题数
    /// </summary>
    public int QuestionCount { get; set; }

    /// <summary>
    /// 作答数
    /// </summary>
    public int AnswerCount { get; set; }

    /// <summary>
    /// 正确数
    /// </summary>
    public int CorrectCount { get; set; }

    /// <summary>
    /// 作答时间
    /// </summary>
    public DateTime CreateTime { get; set; } = DateTime.Now;

    /// <summary>
    /// 用户作答题目
    /// </summary>
    public IReadOnlyList<UserAnswerQuestionGroupModel> QuestionGroups { get; set; }
}

public class UserAnswerQuestionGroupModel
{
    /// <summary>
    /// 题型Id
    /// </summary>
    public Guid QuestionTypeId { get; set; }

    /// <summary>
    /// 用户回答的题目
    /// </summary>
    public IReadOnlyList<UserAnswerQuestionModel> UserAnswerQuestions { get; set; }

    /// <summary>
    /// 判题规则
    /// </summary>
    public Dictionary<ScoringRuleType, decimal> ScoringRules { get; set; }

    /// <summary>
    /// 得分
    /// </summary>
    public decimal Score { get; set; }

    /// <summary>
    /// 答对统计
    /// </summary>
    public int CorrectTotal { get; set; }
}

public class UserAnswerQuestionModel
{
    /// <summary>
    /// 章节Id
    /// </summary>
    public Guid ChapterId { get; set; }

    /// <summary>
    /// 小节Id
    /// </summary>
    public Guid SectionId { get; set; }

    /// <summary>
    /// 知识点Id
    /// </summary>
    public Guid KnowledgePointId { get; set; }

    /// <summary>
    /// 题型Id
    /// </summary>
    public Guid QuestionTypeId { get; set; }

    /// <summary>
    /// 试题Id
    /// </summary>
    public Guid QuestionId { get; set; }

    /// <summary>
    /// 试题父Id
    /// </summary>
    public Guid? ParentId { get; set; }

    /// <summary>
    /// 用户作答
    /// </summary>
    public IReadOnlyList<UserAnswerContent> UserAnswer { get; set; }

    /// <summary>
    /// 判题规则
    /// </summary>
    public Dictionary<ScoringRuleType, decimal> ScoringRules { get; set; }

    /// <summary>
    /// 得分情况
    /// </summary>
    public ScoringResult ScoringResult { get; set; }

    /// <summary>
    /// 是否标记
    /// </summary>
    public bool WhetherMark { get; set; }
}