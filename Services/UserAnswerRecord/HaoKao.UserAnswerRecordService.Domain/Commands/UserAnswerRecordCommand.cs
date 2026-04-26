using HaoKao.Common.Enums;
using HaoKao.UserAnswerRecordService.Domain.Entities;

namespace HaoKao.UserAnswerRecordService.Domain.Commands;

/// <summary>
/// 创建用户答题记录命令
/// </summary>
/// <param name="SubjectId">科目Id</param>
/// <param name="CategoryId">分类Id</param>
/// <param name="RecordIdentifier">章节或试卷的Id</param>
/// <param name="RecordIdentifierName">章节或试卷的名称</param>
/// <param name="ElapsedTime">耗时</param>
/// <param name="PassingScore">及格分数</param>
/// <param name="TotalScore">试题总分</param>
/// <param name="AnswerCount">作答数</param>
/// <param name="CorrectCount">正确数</param>
/// <param name="AnswerType">答题类型</param>
/// <param name="CreateTime">作答时间</param>
/// <param name="CreatorId">学员Id</param>
/// <param name="RecordQuestions">试题作答记录</param>
public record EventCreateUserAnswerRecordCommand(
    Guid SubjectId,
    Guid CategoryId,
    Guid RecordIdentifier,
    string RecordIdentifierName,
    long ElapsedTime,
    decimal PassingScore,
    decimal TotalScore,
    int AnswerCount,
    int CorrectCount,
    SubmitAnswerType AnswerType,
    DateTime CreateTime,
    Guid CreatorId,
    List<UserAnswerQuestion> RecordQuestions
) : Command("创建用户答题记录");