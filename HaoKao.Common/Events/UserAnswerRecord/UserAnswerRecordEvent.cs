using HaoKao.Common.Enums;

namespace HaoKao.Common.Events.UserAnswerRecord;

public record CreateChapterAnswerRecordEvent(
    Guid SubjectId,
    Guid CategoryId,
    Guid ChapterId,
    Guid SectionId,
    Guid KnowledgePointId,
    Guid CreatorId,
    DateTime CreateTime,
    CreateAnswerRecordEvent AnswerRecord
) : IntegrationEvent;

public record CreatePaperAnswerRecordEvent(
    Guid SubjectId,
    Guid CategoryId,
    Guid PaperId,
    Guid CreatorId,
    DateTime CreateTime,
    decimal UserScore,
    decimal PassingScore,
    decimal TotalScore,
    long ElapsedTime,
    CreateAnswerRecordEvent AnswerRecord
) : IntegrationEvent;

public record CreateDateAnswerRecordEvent(
    Guid SubjectId,
    Guid CreatorId,
    DateTime CreateTime,
    DateOnly Date,
    SubmitAnswerType Type,
    CreateAnswerRecordEvent AnswerRecord
) : IntegrationEvent;

public record CreateAnswerRecordEvent(
    int QuestionCount,
    int AnswerCount,
    int CorrectCount,
    DateTime CreateTime,
    Guid CreatorId,
    SubmitAnswerType AnswerType,
    IReadOnlyList<CreateAnswerQuestionEvent> Questions
);

public record CreateAnswerQuestionEvent(
    Guid QuestionId,
    Guid QuestionTypeId,
    Guid? ParentId,
    ScoringRuleType JudgeResult,
    bool WhetherMark,
    IReadOnlyList<CreateUserAnswerEvent> UserAnswers
);

public record CreateUserAnswerEvent(string Content);

public record CreateElapsedTimeRecordEvent(
    Guid SubjectId,
    Guid TargetId,
    Guid ProductId,
    SubmitAnswerType Type,
    int QuestionCount,
    int AnswerCount,
    int CorrectCount,
    long ElapsedSeconds,
    DateTime CreateTime,
    Guid CreatorId
) : IntegrationEvent;