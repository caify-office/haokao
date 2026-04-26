using HaoKao.UserAnswerRecordService.Application.Modules.AnswerRecordModule;
using HaoKao.UserAnswerRecordService.Application.Modules.UserAnswerRecordModule.ViewModels;

namespace HaoKao.UserAnswerRecordService.Application.Modules.UserAnswerRecordModule.Interfaces;

public interface IUserAnswerRecordWebService : IAppWebApiService, IManager
{
    Task<AnswerRecordViewModel> GetChapterAnswerRecord(Guid categoryId, Guid chapterId);

    Task<AnswerRecordViewModel> GetSectionAnswerRecord(Guid categoryId, Guid sectionId);

    Task<AnswerRecordViewModel> GetKnowledgePointAnswerRecord(Guid categoryId, Guid knowledgePointId);

    Task<PaperAnswerRecordViewModel> GetPaperRecord(Guid id);

    Task<IReadOnlyList<ChapterRecordViewModel>> GetChapterList(Guid categoryId, Guid subjectId);

    Task<IReadOnlyList<SectionRecordViewModel>> GetSectionList(Guid categoryId, Guid chapterId);

    Task<IReadOnlyList<KnowledgePointRecordViewModel>> GetKnowledgePointList(Guid categoryId, Guid sectionId);

    Task<IReadOnlyList<PaperRecordViewModel>> GetPaperList(Guid categoryId, Guid subjectId);

    Task<Guid> GetTodayTaskRecordId(Guid subjectId);

    Task<AnswerRecordViewModel> GetTodayTaskRecord(Guid id);

    Task<PracticeAbilityAnalysisViewModel> GetPracticeAbilityAnalysis(Guid subjectId);

    ValueTask RefreshPracticeAbilityAnalysis(Guid subjectId);

    Task<PracticeSituationAnalysisViewModel> GetPracticeSituationAnalysis(Guid subjectId);

    ValueTask RefreshPracticeSituationAnalysis(Guid subjectId);
}