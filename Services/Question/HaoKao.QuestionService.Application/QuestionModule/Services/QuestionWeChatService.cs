using HaoKao.QuestionService.Application.QuestionModule.Interfaces;
using HaoKao.QuestionService.Application.QuestionModule.ViewModels;

namespace HaoKao.QuestionService.Application.QuestionModule.Services;

/// <summary>
/// 试题相关服务--小程序
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.WeChat)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class QuestionWeChatService(IQuestionAppService service) : IQuestionWeChatService
{
    /// <inheritdoc />
    [HttpGet("{id:guid}")]
    public Task<BrowseQuestionAppViewModel> Get(Guid id)
    {
        return service.Get(id);
    }

    /// <inheritdoc />
    [HttpPost, AllowAnonymous]
    public Task<int> GetSubjectQuestionCount
        ([FromBody] QuerySubjectQuestionCountViewModel input)
    {
        return service.GetSubjectQuestionCount(input);
    }

    /// <inheritdoc />
    [HttpGet]
    public Task<IReadOnlyList<ChapterViewModel>> GetChapterList
        ([FromQuery] QueryChapterViewModel input)
    {
        return service.GetChapterList(input);
    }

    /// <inheritdoc />
    [HttpGet]
    public Task<IReadOnlyList<ChapterViewModel>> GetSectionList
        ([FromQuery] QuerySectionViewModel input)
    {
        return service.GetSectionList(input);
    }

    /// <inheritdoc />
    [HttpGet]
    public Task<IReadOnlyList<ChapterViewModel>> GetKnowledgePointList
        ([FromQuery] QueryKnowledgePointViewModel input)
    {
        return service.GetKnowledgePointList(input);
    }

    /// <inheritdoc />
    [HttpGet]
    public Task<Dictionary<Guid, List<QueryQuestionListAppViewModel>>> GetChapterQuestionIdGroup
        ([FromQuery] QueryChapterQuestionViewModel input)
    {
        return service.GetChapterQuestionIdGroup(input);
    }

    /// <inheritdoc />
    [HttpGet]
    public Task<Dictionary<Guid, List<QueryQuestionListAppViewModel>>> GetSectionQuestionIdGroup
        ([FromQuery] QuerySectionQuestionViewModel input)
    {
        return service.GetSectionQuestionIdGroup(input);
    }

    /// <inheritdoc />
    [HttpGet]
    public Task<Dictionary<Guid, List<QueryQuestionListAppViewModel>>> GetKnowledgePointQuestionIdGroup
        ([FromQuery] QueryKnowledgePointQuestionViewModel input)
    {
        return service.GetKnowledgePointQuestionIdGroup(input);
    }

    /// <inheritdoc />
    [HttpPost]
    public Task<Dictionary<Guid, List<QueryQuestionListAppViewModel>>> GetPaperQuestionIdGroup
        ([FromBody] IReadOnlyList<Guid> ids)
    {
        return service.GetPaperQuestionIdGroup(ids);
    }

    /// <inheritdoc />
    [HttpGet]
    public Task<Dictionary<Guid, List<QueryQuestionListAppViewModel>>> GetSpecialPromotionQuestionIdGroup
        ([FromQuery] QuerySpecialPromotionQuestionViewModel input)
    {
        return service.GetSpecialPromotionQuestionIdGroup(input);
    }

    /// <inheritdoc />
    [HttpGet]
    public async Task<bool> ExistsFreeQuestion(Guid subjectId, Guid chapterId, Guid categoryId)
    {
        return await service.ExistsFreeQuestion(subjectId, chapterId, categoryId);
    }

    /// <inheritdoc />
    [HttpPost]
    public Task<SubmitAnswerReturnViewModel> SubmitUserAnswers([FromBody] SubmitAnswersViewModel input)
    {
        return service.SubmitUserAnswers(input);
    }
}