using HaoKao.QuestionService.Application.QuestionModule.ViewModels;
using HaoKao.QuestionService.Application.QuestionWrongModule.Interfaces;
using HaoKao.QuestionService.Application.QuestionWrongModule.ViewModels;
using QueryChapterQuestionViewModel = HaoKao.QuestionService.Application.QuestionWrongModule.ViewModels.QueryChapterQuestionViewModel;
using QueryChapterViewModel = HaoKao.QuestionService.Application.QuestionWrongModule.ViewModels.QueryChapterViewModel;

namespace HaoKao.QuestionService.Application.QuestionWrongModule.Services;

/// <summary>
/// 错题相关服务--小程序
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.WeChat)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class QuestionWrongWeChatService(IQuestionWrongAppService appService) : IQuestionWrongWeChatService
{
    /// <inheritdoc />
    [HttpGet]
    public Task<IReadOnlyList<ChapterViewModel>> GetChapterList([FromQuery] QueryChapterViewModel input)
    {
        return appService.GetChapterList(input);
    }

    /// <inheritdoc />
    [HttpGet]
    public Task<Dictionary<Guid, List<QueryQuestionListAppViewModel>>> GetChapterQuestionIdGroup([FromQuery] QueryChapterQuestionViewModel input)
    {
        return appService.GetChapterQuestionIdGroup(input);
    }

    /// <inheritdoc />
    [HttpGet("{subjectId:guid}")]
    public Task<bool> Any(Guid subjectId)
    {
        return appService.Any(subjectId);
    }

    /// <inheritdoc />
    [HttpGet]
    public Task<Dictionary<Guid, List<QueryQuestionListAppViewModel>>> GetTodayTask([FromQuery] QueryTodayTaskViewModel input)
    {
        return appService.GetTodayTask(input);
    }

    /// <inheritdoc />
    [HttpPost]
    public Task<QueryMakingPaperViewModel> GetMakingPaper([FromBody] QueryMakingPaperViewModel input)
    {
        return appService.GetMakingPaper(input);
    }
}