using HaoKao.QuestionService.Application.DailyQuestionModule.Interfaces;
using HaoKao.QuestionService.Application.DailyQuestionModule.ViewModels;

namespace HaoKao.QuestionService.Application.DailyQuestionModule.Services;

/// <summary>
/// App 端服务接口
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.App)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class DailyQuestionAppService(IDailyQuestionService service) : IDailyQuestionAppService
{
    /// <inheritdoc />
    [HttpGet("{subjectId:guid}/{dateTime:datetime}")]
    public Task<DailyQuestionViewModel> GetCurrentDailyQuestion(Guid subjectId, DateTime dateTime)
    {
        return service.Get(subjectId, dateTime);
    }
}