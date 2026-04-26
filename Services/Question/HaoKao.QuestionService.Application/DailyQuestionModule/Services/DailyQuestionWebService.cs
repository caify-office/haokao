using HaoKao.QuestionService.Application.DailyQuestionModule.Interfaces;
using HaoKao.QuestionService.Application.DailyQuestionModule.ViewModels;

namespace HaoKao.QuestionService.Application.DailyQuestionModule.Services;

[DynamicWebApi(Module = ServiceAreaName.WebSite)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class DailyQuestionWebService(IDailyQuestionService service) : IDailyQuestionWebService
{
    /// <inheritdoc />
    [HttpGet("{subjectId:guid}/{dateTime:datetime}")]
    public Task<DailyQuestionViewModel> Get(Guid subjectId, DateTime dateTime)
    {
        return service.Get(subjectId, dateTime);
    }
}