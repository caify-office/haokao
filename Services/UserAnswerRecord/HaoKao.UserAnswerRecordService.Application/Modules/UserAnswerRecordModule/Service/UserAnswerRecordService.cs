using HaoKao.UserAnswerRecordService.Application.Modules.AnswerRecordModule;
using HaoKao.UserAnswerRecordService.Application.Modules.UserAnswerRecordModule.Interfaces;
using HaoKao.UserAnswerRecordService.Application.Modules.UserAnswerRecordModule.ViewModels;
using HaoKao.UserAnswerRecordService.Domain.Repositories;
using HaoKao.UserAnswerRecordService.Domain.Works;

namespace HaoKao.UserAnswerRecordService.Application.Modules.UserAnswerRecordModule.Service;

[DynamicWebApi(Module = ServiceAreaName.Management)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsJwt)]
public class UserAnswerRecordService(
    IChapterAnswerRecordService chapterService,
    IPaperAnswerRecordService paperService,
    IDataAnalysisService analysisService,
    IAnswerRecordRepository repository
) : IUserAnswerRecordService
{
    /// <summary>
    /// 获取章节列表
    /// </summary>
    /// <param name="categoryId">类别Id</param>
    /// <param name="subjectId">科目Id</param>
    /// <returns></returns>
    [HttpGet("{categoryId:guid}/{subjectId:guid}")]
    public Task<IReadOnlyList<ChapterRecordViewModel>> GetChapterList(Guid categoryId, Guid subjectId)
    {
        return chapterService.GetChapterList(categoryId, subjectId);
    }

    /// <summary>
    /// 获取小节列表
    /// </summary>
    /// <param name="categoryId">类别Id</param>
    /// <param name="chapterId">章节Id</param>
    /// <returns></returns>
    [HttpGet("{categoryId:guid}/{chapterId:guid}")]
    public Task<IReadOnlyList<SectionRecordViewModel>> GetSectionList(Guid categoryId, Guid chapterId)
    {
        return chapterService.GetSectionList(categoryId, chapterId);
    }

    /// <summary>
    /// 获取知识点列表
    /// </summary>
    /// <param name="categoryId">类别Id</param>
    /// <param name="sectionId">小节Id</param>
    /// <returns></returns>
    [HttpGet("{categoryId:guid}/{sectionId:guid}")]
    public Task<IReadOnlyList<KnowledgePointRecordViewModel>> GetKnowledgePointList(Guid categoryId, Guid sectionId)
    {
        return chapterService.GetKnowledgePointList(categoryId, sectionId);
    }

    /// <summary>
    /// 获取试卷列表
    /// </summary>
    /// <param name="categoryId">类别Id</param>
    /// <param name="subjectId">科目Id</param>
    /// <returns></returns>
    [HttpGet("{categoryId:guid}/{subjectId:guid}")]
    public Task<IReadOnlyList<PaperRecordViewModel>> GetPaperList(Guid categoryId, Guid subjectId)
    {
        return paperService.GetPaperList(categoryId, subjectId);
    }

    /// <summary>
    /// 获取已答题数
    /// </summary>
    /// <param name="subjectId"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    [HttpGet("{subjectId:guid}")]
    public Task<int> GetAnsweredQuestionCount(Guid subjectId, Guid userId)
    {
        return repository.GetSubjectQuestionCount(subjectId, userId);
    }

    /// <summary>
    /// 获取做题能力分析
    /// </summary>
    /// <param name="subjectId"></param>
    /// <param name="userId"></param>
    [HttpGet("{subjectId:guid}")]
    public Task<PracticeAbilityAnalysisViewModel> GetPracticeAbilityAnalysis(Guid subjectId, Guid userId)
    {
        return analysisService.GetPracticeAbilityAnalysis(subjectId, userId);
    }

    /// <summary>
    /// 刷新做题能力分析
    /// </summary>
    /// <param name="subjectId"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    [HttpGet("{subjectId:guid}")]
    public ValueTask RefreshPracticeAbilityAnalysis(Guid subjectId, Guid userId)
    {
        return analysisService.RefreshPracticeAbilityAnalysis(subjectId, userId);
    }

    /// <summary>
    /// 获取练习状况分析
    /// </summary>
    /// <param name="subjectId"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    [HttpGet("{subjectId:guid}")]
    public Task<PracticeSituationAnalysisViewModel> GetPracticeSituationAnalysis(Guid subjectId, Guid userId)
    {
        return analysisService.GetPracticeSituationAnalysis(subjectId, userId);
    }

    /// <summary>
    /// 刷新练习状况分析
    /// </summary>
    /// <param name="subjectId"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    [HttpGet("{subjectId:guid}")]
    public ValueTask RefreshPracticeSituationAnalysis(Guid subjectId, Guid userId)
    {
        return analysisService.RefreshPracticeSituationAnalysis(subjectId, userId);
    }

    [HttpGet, AllowAnonymous]
    public Task UnionAnswerRecordWork()
    {
        return EngineContext.Current.Resolve<IUnionAnswerRecordWork>().ExecuteAsync();
    }

    [HttpGet, AllowAnonymous]
    public Task MigrateRecordDataWork([FromServices] IMigrateRecordDataWork work)
    {
        return work.ExecuteAsync();
    }
}