using HaoKao.Common.Enums;
using HaoKao.UserAnswerRecordService.Domain.Entities;

namespace HaoKao.UserAnswerRecordService.Domain.Commands;

/// <summary>
/// 创建产品章节作答记录
/// </summary>
/// <param name="ProductId">产品Id</param>
/// <param name="SubjectId">科目Id</param>
/// <param name="ChapterId">章节Id</param>
/// <param name="SectionId">小节Id</param>
/// <param name="KnowledgePointId">知识点Id</param>
/// <param name="CreatorId">用户Id</param>
/// <param name="CreateTime">提交时间</param>
/// <param name="AnswerRecord">作答记录</param>
public record EventCreateProductChapterAnswerRecordCommand(
    Guid ProductId,
    Guid SubjectId,
    Guid ChapterId,
    Guid SectionId,
    Guid KnowledgePointId,
    Guid CreatorId,
    DateTime CreateTime,
    AnswerRecord AnswerRecord
) : Command("创建产品知识点作答记录");

/// <summary>
/// 创建产品试卷作答记录
/// </summary>
/// <param name="ProductId">产品Id</param>
/// <param name="SubjectId">科目Id</param>
/// <param name="PaperId">试卷Id</param>
/// <param name="UserScore">用户得分</param>
/// <param name="PassingScore">及格分数</param>
/// <param name="TotalScore">试卷总分</param>
/// <param name="CreatorId">用户Id</param>
/// <param name="CreateTime">提交时间</param>
/// <param name="AnswerRecord">作答记录</param>
public record EventCreateProductPaperAnswerRecordCommand(
    Guid ProductId,
    Guid SubjectId,
    Guid PaperId,
    decimal UserScore,
    decimal PassingScore,
    decimal TotalScore,
    Guid CreatorId,
    DateTime CreateTime,
    AnswerRecord AnswerRecord
) : Command("创建产品试卷作答记录");

/// <summary>
/// 创建产品知识点作答记录
/// </summary>
/// <param name="ProductId">产品Id</param>
/// <param name="SubjectId">科目Id</param>
/// <param name="ChapterId">章节Id</param>
/// <param name="SectionId">小节Id</param>
/// <param name="KnowledgePointId">知识点Id</param>
/// <param name="CreatorId">用户Id</param>
/// <param name="CreateTime">提交时间</param>
/// <param name="ExamFrequency">考试频率</param>
/// <param name="AnswerRecord">作答记录</param>
public record EventCreateProductKnowledgeAnswerRecordCommand(
    Guid ProductId,
    Guid SubjectId,
    Guid ChapterId,
    Guid SectionId,
    Guid KnowledgePointId,
    Guid CreatorId,
    DateTime CreateTime,
    ExamFrequency ExamFrequency,
    AnswerRecord AnswerRecord
) : Command("创建产品知识点作答记录");