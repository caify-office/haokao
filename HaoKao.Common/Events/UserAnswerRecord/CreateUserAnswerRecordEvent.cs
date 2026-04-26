using HaoKao.Common.Enums;

namespace HaoKao.Common.Events.UserAnswerRecord;

public record CreateUserAnswerRecordEvent(
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
    List<CreateUserAnswerRecordQuestionEvent> RecordQuestions
) : IntegrationEvent;

public record CreateUserAnswerRecordQuestionEvent(
    Guid QuestionId,
    Guid? ParentId,
    Guid QuestionTypeId,
    bool WhetherMark,
    decimal UserScore,
    ScoringRuleType JudgeResult,
    List<CreateUserAnswerRecordQuestionOptionEvent> QuestionOptions
);

public record CreateUserAnswerRecordQuestionOptionEvent(Guid OptionId, string OptionContent);