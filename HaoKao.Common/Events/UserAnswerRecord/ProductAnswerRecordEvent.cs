namespace HaoKao.Common.Events.UserAnswerRecord;

public record CreateProductChapterAnswerRecordEvent(
    Guid ProductId,
    Guid SubjectId,
    Guid ChapterId,
    Guid SectionId,
    Guid KnowledgePointId,
    Guid CreatorId,
    DateTime CreateTime,
    CreateAnswerRecordEvent AnswerRecord
) : IntegrationEvent;

public record CreateProductPaperAnswerRecordEvent(
    Guid ProductId,
    Guid SubjectId,
    Guid PaperId,
    decimal UserScore,
    decimal PassingScore,
    decimal TotalScore,
    Guid CreatorId,
    DateTime CreateTime,
    CreateAnswerRecordEvent AnswerRecord
) : IntegrationEvent;

public record CreateProductKnowledgeAnswerRecordEvent(
    Guid ProductId,
    Guid SubjectId,
    Guid ChapterId,
    Guid SectionId,
    Guid KnowledgePointId,
    Guid CreatorId,
    DateTime CreateTime,
    int ExamFrequency,
    CreateAnswerRecordEvent AnswerRecord
) : IntegrationEvent;