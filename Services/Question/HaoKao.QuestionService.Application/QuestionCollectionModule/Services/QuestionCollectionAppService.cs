using HaoKao.QuestionService.Application.QuestionCollectionModule.Interfaces;
using HaoKao.QuestionService.Application.QuestionCollectionModule.ViewModels;

namespace HaoKao.QuestionService.Application.QuestionCollectionModule.Services;

/// <summary>
/// 试题收藏相关服务 --App端
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.App)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class QuestionCollectionAppService(IQuestionCollectionService service) : IQuestionCollectionAppService
{
    private readonly IQuestionCollectionService _service = service ?? throw new ArgumentNullException(nameof(service));

    /// <summary>
    /// 获取收藏的试题列表，带分页、带条件查询
    /// </summary>
    /// <param name="viewModel"></param>
    /// <returns></returns>
    [HttpGet]
    public Task<QueryQuestionCollectionViewModel> Get([FromQuery] QueryQuestionCollectionViewModel viewModel)
    {
        return _service.Get(viewModel);
    }

    /// <summary>
    /// 获取当前用户试题收藏分类统计
    /// </summary>
    /// <param name="subjectId"></param>
    /// <returns></returns>
    [HttpGet("{subjectId:guid}")]
    public Task<List<QuestionCollectionStatViewModel>> GetCollectionQuestionTypeCount(Guid subjectId)
    {
        return _service.Get(subjectId);
    }

    /// <summary>
    /// 是否收藏
    /// </summary>
    /// <param name="questionId"></param>
    /// <returns></returns>
    [HttpGet]
    public Task<bool> IsCollected(Guid questionId)
    {
        return _service.Any(questionId);
    }

    /// <summary>
    /// 收藏试题
    /// </summary>
    /// <param name="questionId"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<bool> CollectionQuestion([FromBody] Guid questionId)
    {
        await _service.Create(questionId);
        return true;
    }

    /// <summary>
    /// 取消收藏试题
    /// </summary>
    /// <param name="questionId"></param>
    /// <returns></returns>
    [HttpDelete("{questionId:guid}")]
    public async Task<bool> UnCollectionQuestion(Guid questionId)
    {
        await _service.Delete(questionId);
        return true;
    }
}