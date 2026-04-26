using HaoKao.QuestionService.Application.QuestionCollectionModule.Interfaces;
using HaoKao.QuestionService.Application.QuestionModule.Interfaces;
using HaoKao.QuestionService.Application.QuestionModule.ViewModels;

namespace HaoKao.QuestionService.Application.QuestionModule.Services;

[DynamicWebApi(Module = ServiceAreaName.WebSite)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class QuestionWebService(
    IQuestionAppService appService,
    IQuestionCollectionWebService questionCollectionWebService,
    IMapper mapper
) : IQuestionWebService
{
    #region 初始参数

    private readonly IQuestionAppService _appService = appService ?? throw new ArgumentNullException(nameof(appService));
    private readonly IQuestionCollectionWebService _questionCollectionWebService = questionCollectionWebService ?? throw new ArgumentNullException(nameof(questionCollectionWebService));
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

    #endregion

    #region 服务方法

    /// <inheritdoc />
    [HttpGet]
    public Task<IReadOnlyList<ChapterViewModel>> GetChapterList([FromQuery] QueryChapterViewModel input)
    {
        return _appService.GetChapterList(input);
    }

    /// <inheritdoc />
    public Task<IReadOnlyList<ChapterViewModel>> GetSectionList([FromQuery] QuerySectionViewModel input)
    {
        return _appService.GetSectionList(input);
    }

    /// <inheritdoc />
    public Task<IReadOnlyList<ChapterViewModel>> GetKnowledgePointList([FromQuery] QueryKnowledgePointViewModel input)
    {
        return _appService.GetKnowledgePointList(input);
    }

    /// <inheritdoc />
    [HttpGet]
    public async Task<Dictionary<Guid, List<BrowseQuestionAppViewModel>>> GetChapterQuestionList
        ([FromQuery] QueryChapterQuestionViewModel input)
    {
        var questions = await _appService.GetChapterQuestionList(input);
        var list = _mapper.Map<List<BrowseQuestionAppViewModel>>(questions);
        await _questionCollectionWebService.SetIsCollected(list);
        return _appService.GroupByQuestionType(list);
    }

    /// <inheritdoc />
    public async Task<Dictionary<Guid, List<BrowseQuestionAppViewModel>>> GetSectionQuestionList
        ([FromQuery] QuerySectionQuestionViewModel input)
    {
        var questions = await _appService.GetSectionQuestionList(input);
        var list = _mapper.Map<List<BrowseQuestionAppViewModel>>(questions);
        await _questionCollectionWebService.SetIsCollected(list);
        return _appService.GroupByQuestionType(list);
    }

    /// <inheritdoc />
    [HttpGet]
    public async Task<Dictionary<Guid, List<BrowseQuestionAppViewModel>>> GetKnowledgePointQuestionList
        ([FromQuery] QueryKnowledgePointQuestionViewModel input)
    {
        var questions = await _appService.GetKnowledgePointQuestionList(input);
        var list = _mapper.Map<List<BrowseQuestionAppViewModel>>(questions);
        await _questionCollectionWebService.SetIsCollected(list);
        return _appService.GroupByQuestionType(list);
    }

    /// <inheritdoc />
    [HttpPost]
    public async Task<Dictionary<Guid, List<BrowseQuestionAppViewModel>>> GetPaperQuestionList([FromBody] IReadOnlyList<Guid> ids)
    {
        var questions = await _appService.GetQuestionListByIds(ids);
        var list = _mapper.Map<List<BrowseQuestionAppViewModel>>(questions);
        await _questionCollectionWebService.SetIsCollected(list);
        return _appService.GroupByQuestionType(list);
    }

    /// <inheritdoc />
    [HttpGet]
    public async Task<Dictionary<Guid, List<BrowseQuestionAppViewModel>>> GetSpecialPromotionQuestionList
        ([FromQuery] QuerySpecialPromotionQuestionViewModel input)
    {
        var ids = await _appService.GetSpecialPromotionQuestionIds(input);
        var questions = await _appService.GetQuestionListByIds(ids);
        var list = _mapper.Map<List<BrowseQuestionAppViewModel>>(questions);
        await _questionCollectionWebService.SetIsCollected(list);
        return _appService.GroupByQuestionType(list);
    }

    /// <inheritdoc />
    [HttpGet]
    public async Task<bool> ExistsFreeQuestion(Guid subjectId, Guid chapterId, Guid cagetoryId)
    {
        return await _appService.ExistsFreeQuestion(subjectId, chapterId, cagetoryId);
    }

    /// <inheritdoc />
    [HttpPost]
    public Task<SubmitAnswerReturnViewModel> SubmitUserAnswers(SubmitAnswersViewModel vm)
    {
        return _appService.SubmitUserAnswers(vm);
    }

    #endregion
}