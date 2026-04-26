using HaoKao.QuestionService.Application.QuestionCollectionModule.Interfaces;
using HaoKao.QuestionService.Application.QuestionModule.Interfaces;
using HaoKao.QuestionService.Application.QuestionModule.ViewModels;
using HaoKao.QuestionService.Application.QuestionWrongModule.Interfaces;
using HaoKao.QuestionService.Application.QuestionWrongModule.ViewModels;
using QueryChapterQuestionViewModel = HaoKao.QuestionService.Application.QuestionWrongModule.ViewModels.QueryChapterQuestionViewModel;
using QueryChapterViewModel = HaoKao.QuestionService.Application.QuestionWrongModule.ViewModels.QueryChapterViewModel;

namespace HaoKao.QuestionService.Application.QuestionWrongModule.Services;

[DynamicWebApi(Module = ServiceAreaName.WebSite)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class QuestionWrongWebService(
    IQuestionWrongService service,
    IQuestionAppService questionAppService,
    IQuestionCollectionWebService collectionWebService,
    IMapper mapper
) : IQuestionWrongWebService
{
    /// <inheritdoc />
    [HttpGet]
    public Task<IReadOnlyList<ChapterViewModel>> GetChapterList([FromQuery] QueryChapterViewModel input)
    {
        return service.GetChapterList(input);
    }

    /// <inheritdoc />
    [HttpGet]
    public async Task<Dictionary<Guid, List<BrowseQuestionAppViewModel>>> GetChapterQuestionList([FromQuery] QueryChapterQuestionViewModel input)
    {
        var questions = await service.GetChapterQuestionList(input);
        var list = mapper.Map<List<BrowseQuestionAppViewModel>>(questions);
        return questionAppService.GroupByQuestionType(list);
    }

    /// <inheritdoc />
    [HttpGet("{subjectId:guid}")]
    public Task<bool> Any(Guid subjectId)
    {
        return service.Any(subjectId);
    }

    /// <inheritdoc />
    [HttpGet]
    public async Task<Dictionary<Guid, List<BrowseQuestionAppViewModel>>> GetTodayTask([FromQuery] QueryTodayTaskViewModel input)
    {
        var ids = await service.GetTodayTaskQuestionIds(input);
        if (ids.Count == 0) return null;
        var questions = await questionAppService.GetQuestionListByIds(ids);
        var list = mapper.Map<List<BrowseQuestionAppViewModel>>(questions);
        await collectionWebService.SetIsCollected(list);
        return questionAppService.GroupByQuestionType(list);
    }
}