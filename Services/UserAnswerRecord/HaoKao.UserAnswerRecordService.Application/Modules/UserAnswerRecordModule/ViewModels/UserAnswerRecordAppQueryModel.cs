using HaoKao.UserAnswerRecordService.Domain.Queries;

namespace HaoKao.UserAnswerRecordService.Application.Modules.UserAnswerRecordModule.ViewModels;

[AutoMapTo(typeof(UserAnswerRecordQuery))]
public class UserAnswerRecordAppQueryModel : IDto
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
    public List<Guid> RecordIdentifier { get; set; }

    /// <summary>
    /// 用户Id
    /// </summary>
    public Guid? UserId { get; set; } = EngineContext.Current.IsAuthenticated
        ? EngineContext.Current.ClaimManager.IdentityClaim.GetUserId<Guid>()
        : null;
}