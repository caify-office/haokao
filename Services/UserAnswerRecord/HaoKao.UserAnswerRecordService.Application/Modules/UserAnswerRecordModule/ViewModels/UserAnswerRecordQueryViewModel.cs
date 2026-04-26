using HaoKao.Common.Enums;
using HaoKao.UserAnswerRecordService.Domain.Entities;
using HaoKao.UserAnswerRecordService.Domain.Queries;

namespace HaoKao.UserAnswerRecordService.Application.Modules.UserAnswerRecordModule.ViewModels;

[AutoMapFrom(typeof(UserAnswerRecordQuery))]
[AutoMapTo(typeof(UserAnswerRecordQuery))]
public class UserAnswerRecordQueryViewModel : QueryDtoBase<UserAnswerRecordQueryListViewModel>
{
    /// <summary>
    /// 科目Id
    /// </summary>
    public Guid? SubjectId { get; set; }

    /// <summary>
    /// 试题分类Id
    /// </summary>
    public Guid? QuestionCategoryId { get; set; }

    /// <summary>
    /// 答题标识符 章节Id，或试卷Id，每日一练和消灭错题 为Guid.Empty
    /// </summary>
    public List<Guid> RecordIdentifier { get; set; } = [];

    /// <summary>
    /// 答题类型
    /// </summary>
    public SubmitAnswerType? AnswerType { get; set; }

    /// <summary>
    /// 答题开始时间
    /// </summary>
    public DateTime? StartDateTime { get; set; }

    /// <summary>
    /// 答题结束时间
    /// </summary>
    public DateTime? EndDateTime { get; set; }

    /// <summary>
    /// 用户Id
    /// </summary>
    public Guid? UserId { get; set; } = EngineContext.Current.IsAuthenticated
        ? EngineContext.Current.ClaimManager.IdentityClaim.GetUserId<Guid>()
        : null;
}

[AutoMapTo(typeof(UserAnswerRecord))]
[AutoMapFrom(typeof(UserAnswerRecord))]
public class UserAnswerRecordQueryListViewModel : IDto
{
    /// <summary>
    /// Id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// 科目Id
    /// </summary>
    public Guid SubjectId { get; set; }

    /// <summary>
    /// 试题分类Id
    /// </summary>
    public Guid QuestionCategoryId { get; set; }

    /// <summary>
    /// 答题标识符 章节Id，或试卷Id，每日一练和消灭错题 为Guid.Empty
    /// </summary>
    public Guid RecordIdentifier { get; set; }

    /// <summary>
    /// 章节名称或者试卷名称
    /// </summary>
    public string RecordIdentifierName { get; set; }

    /// <summary>
    /// 用户Id
    /// </summary>
    public Guid CreatorId { get; set; }

    /// <summary>
    /// 答题类型
    /// </summary>
    public SubmitAnswerType AnswerType { get; set; }

    /// <summary>
    /// 用户得分
    /// </summary>
    public decimal UserScore { get; set; }

    /// <summary>
    /// 及格分数
    /// </summary>
    public decimal PassingScore { get; set; }

    /// <summary>
    /// 试题总分
    /// </summary>
    public decimal TotalScore { get; set; }

    /// <summary>
    /// 耗时
    /// </summary>
    public long ElapsedTime { get; set; }

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
    /// 提交答题时间
    /// </summary>
    public DateTime CreateTime { get; set; }
}