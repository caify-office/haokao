using HaoKao.QuestionService.Application.DailyQuestionModule.Interfaces;
using HaoKao.QuestionService.Application.DailyQuestionModule.ViewModels;

namespace HaoKao.QuestionService.Application.DailyQuestionModule.Services;

/// <summary>
/// 每日一题服务--小程序
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.WeChat)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class DailyQuestionWeChatService(IDailyQuestionService service) : IDailyQuestionWeChatService
{
    /// <inheritdoc />
    [HttpGet("{subjectId:guid}/{dateTime:datetime}")]
    public Task<DailyQuestionViewModel> Get(Guid subjectId, DateTime dateTime)
    {
        return service.Get(subjectId, dateTime);
    }
}