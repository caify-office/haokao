using HaoKao.Common.Enums;
using HaoKao.UserAnswerRecordService.Domain.Entities;

namespace HaoKao.UserAnswerRecordService.Domain.Commands;

/// <summary>
/// 创建章节答题记录命令
/// </summary>
/// <param name="SubjectId">科目Id</param>
/// <param name="CategoryId">类别Id</param>
/// <param name="ChapterId">章节Id</param>
/// <param name="SectionId">小节Id</param>
/// <param name="KnowledgePointId">知识点Id</param>
/// <param name="CreatorId">用户Id</param>
/// <param name="CreateTime">提交时间</param>
/// <param name="AnswerRecord">作答记录</param>
public record EventCreateChapterAnswerRecordCommand(
    Guid SubjectId,
    Guid CategoryId,
    Guid ChapterId,
    Guid SectionId,
    Guid KnowledgePointId,
    Guid CreatorId,
    DateTime CreateTime,
    AnswerRecord AnswerRecord
) : Command("创建章节答题记录");

/// <summary>
/// 创建试卷作答记录命令
/// </summary>
/// <param name="SubjectId">科目Id</param>
/// <param name="CategoryId">类别Id</param>
/// <param name="PaperId">试卷Id</param>
/// <param name="UserScore">用户得分</param>
/// <param name="PassingScore">及格分</param>
/// <param name="TotalScore">总分</param>
/// <param name="CreatorId">用户Id</param>
/// <param name="CreateTime">提交时间</param>
/// <param name="ElapsedTime">耗时(秒)</param>
/// <param name="AnswerRecord">作答记录</param>
public record EventCreatePaperAnswerRecordCommand(
    Guid SubjectId,
    Guid CategoryId,
    Guid PaperId,
    Guid CreatorId,
    DateTime CreateTime,
    decimal UserScore,
    decimal PassingScore,
    decimal TotalScore,
    long ElapsedTime,
    AnswerRecord AnswerRecord
) : Command("创建试卷作答记录");

/// <summary>
/// 创建日期相关作答记录命令
/// </summary>
/// <param name="SubjectId">科目Id</param>
/// <param name="Type">类别</param>
/// <param name="Date">日期</param>
/// <param name="CreatorId">用户Id</param>
/// <param name="CreateTime">提交时间</param>
/// <param name="AnswerRecord">作答记录</param>
public record EventCreateDateAnswerRecordCommand(
    Guid SubjectId,
    Guid CreatorId,
    DateTime CreateTime,
    DateOnly Date,
    SubmitAnswerType Type,
    AnswerRecord AnswerRecord
) : Command("创建日期相关作答记录");

/// <summary>
/// 创建做题耗时记录
/// </summary>
/// <param name="SubjectId">科目Id</param>
/// <param name="TargetId">目标Id</param>
/// <param name="ProductId">产品Id</param>
/// <param name="Type">作答类型</param>
/// <param name="QuestionCount">总题数</param>
/// <param name="AnswerCount">答题数</param>
/// <param name="CorrectCount">正确数</param>
/// <param name="ElapsedSeconds">耗时(秒)</param>
/// <param name="CreatorId">用户Id</param>
/// <param name="CreateTime">做题时间</param>
public record EventCreateElapsedTimeRecordCommand(
    Guid SubjectId,
    Guid TargetId,
    Guid ProductId,
    SubmitAnswerType Type,
    int QuestionCount,
    int AnswerCount,
    int CorrectCount,
    long ElapsedSeconds,
    Guid CreatorId,
    DateTime CreateTime
) : Command("创建做题耗时记录");