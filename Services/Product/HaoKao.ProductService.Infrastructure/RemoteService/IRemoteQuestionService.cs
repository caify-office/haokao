using Girvs.Refit;
using HaoKao.Common.RemoteService;
using Refit;

namespace HaoKao.ProductService.Infrastructure.RemoteService;

[RefitService(RefitServiceNames.QuestionService)]
public interface IRemoteQuestionService : IGirvsRefit
{
    [Get($"{URLPrefixManager.QuestionManagementPrefix}/ChaperCategorieQuestionCount/{{chaperId}}/{{questionCategoryId}}")]
    Task<int> GetChaperCategorieQuestionCount(Guid chaperId, Guid questionCategoryId);
}