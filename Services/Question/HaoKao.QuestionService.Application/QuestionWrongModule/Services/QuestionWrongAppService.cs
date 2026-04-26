using HaoKao.QuestionService.Application.QuestionModule.Interfaces;
using HaoKao.QuestionService.Application.QuestionModule.ViewModels;
using HaoKao.QuestionService.Application.QuestionWrongModule.Interfaces;
using HaoKao.QuestionService.Application.QuestionWrongModule.ViewModels;
using HaoKao.QuestionService.Domain.QuestionWrongModule;
using QueryChapterQuestionViewModel = HaoKao.QuestionService.Application.QuestionWrongModule.ViewModels.QueryChapterQuestionViewModel;
using QueryChapterViewModel = HaoKao.QuestionService.Application.QuestionWrongModule.ViewModels.QueryChapterViewModel;

namespace HaoKao.QuestionService.Application.QuestionWrongModule.Services;

/// <summary>
/// 错题集服务--App端
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.App)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class QuestionWrongAppService(
    IMapper mapper,
    IQuestionWrongRepository repository,
    IQuestionWrongService service,
    IQuestionAppService questionAppService
) : IQuestionWrongAppService
{
    /// <inheritdoc />
    [HttpGet]
    public Task<IReadOnlyList<ChapterViewModel>> GetChapterList([FromQuery] QueryChapterViewModel input)
    {
        return service.GetChapterList(input);
    }

    /// <inheritdoc />
    [HttpGet]
    public async Task<Dictionary<Guid, List<QueryQuestionListAppViewModel>>> GetChapterQuestionIdGroup([FromQuery] QueryChapterQuestionViewModel input)
    {
        var questions = await service.GetChapterQuestionList(input);
        var list = mapper.Map<List<QueryQuestionListAppViewModel>>(questions);
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
    public async Task<Dictionary<Guid, List<QueryQuestionListAppViewModel>>> GetTodayTask([FromQuery] QueryTodayTaskViewModel input)
    {
        var ids = await service.GetTodayTaskQuestionIds(input);
        if (!ids.Any()) return null;
        var questions = await questionAppService.GetQuestionListByIds(ids);
        var list = mapper.Map<List<QueryQuestionListAppViewModel>>(questions);
        return questionAppService.GroupByQuestionType(list);
    }

    /// <inheritdoc />
    [HttpGet]
    public async Task<QueryMakingPaperViewModel> GetMakingPaper([FromQuery] QueryMakingPaperViewModel input)
    {
        var query = input.MapToQuery<QuestionWrongQuery>();
        query.OrderBy = nameof(QuestionWrong.CreateTime);
        await repository.GetByQueryAsync(query);
        return query.MapToQueryDto<QueryMakingPaperViewModel, QuestionWrong>();
    }
}