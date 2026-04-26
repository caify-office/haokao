using HaoKao.QuestionService.Application.QuestionCollectionModule.Interfaces;
using HaoKao.QuestionService.Application.QuestionCollectionModule.ViewModels;
using HaoKao.QuestionService.Application.QuestionModule.Interfaces;
using HaoKao.QuestionService.Application.QuestionModule.ViewModels;
using HaoKao.QuestionService.Domain.CacheExtensions;
using HaoKao.QuestionService.Domain.QuestionCollectionModule;
using HaoKao.QuestionService.Domain.QuestionModule;

namespace HaoKao.QuestionService.Application.QuestionCollectionModule.Services;

[DynamicWebApi(Module = ServiceAreaName.WebSite)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class QuestionCollectionWebService(
    IStaticCacheManager cacheManager,
    IQuestionCollectionService service,
    IQuestionAppService questionAppService,
    IQuestionCollectionRepository repository,
    IMapper mapper
) : IQuestionCollectionWebService
{
    #region 初始参数

    private readonly IStaticCacheManager _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
    private readonly IQuestionCollectionService _service = service ?? throw new ArgumentNullException(nameof(service));
    private readonly IQuestionAppService _questionAppService = questionAppService ?? throw new ArgumentNullException(nameof(questionAppService));
    private readonly IQuestionCollectionRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

    #endregion

    #region 服务方法

    /// <summary>
    /// 获取当前用户试题收藏分类统计
    /// </summary>
    /// <returns></returns>
    [HttpGet("{subjectId:guid}")]
    public Task<List<QuestionCollectionStatViewModel>> Get(Guid subjectId)
    {
        return _service.Get(subjectId);
    }

    /// <summary>
    /// 根据科目和题型获取收藏的试题
    /// </summary>
    /// <param name="subjectId"></param>
    /// <param name="typeId"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<Dictionary<Guid, List<BrowseQuestionAppViewModel>>> Get(Guid subjectId, Guid? typeId)
    {
        var userId = EngineContext.Current.ClaimManager.GetUserId().To<Guid>();
        var query = _repository.Query.Include(x => x.Question)
                               .Where(x => x.CreatorId == userId)
                               .Where(x => x.Question.SubjectId == subjectId)
                               .Where(x => x.Question.EnableState == EnableState.Enable);

        if (typeId.HasValue)
        {
            if (QuestionType.CaseAnalysis == typeId)
            {
                query = query.Where(x => x.Question.ParentId != null);
            }
            else
            {
                query = query.Where(x => x.Question.QuestionTypeId == typeId.Value)
                             .Where(x => x.Question.ParentId == null);
            }
        }

        var ids = await query.Select(x => x.QuestionId).ToListAsync();
        var questions = await _questionAppService.GetQuestionListByIds(ids);
        var list = _mapper.Map<List<BrowseQuestionAppViewModel>>(questions);
        await SetIsCollected(list);
        return _questionAppService.GroupByQuestionType(list);
    }

    /// <summary>
    /// 收藏试题
    /// </summary>
    /// <param name="questionId"></param>
    /// <returns></returns>
    [HttpPost("{questionId:guid}")]
    public async Task<bool> Create(Guid questionId)
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
    public async Task<bool> Delete(Guid questionId)
    {
        await _service.Delete(questionId);
        return true;
    }

    /// <summary>
    /// 试题列表添加是否收藏状态
    /// </summary>
    /// <param name="list"></param>
    [NonAction]
    public async Task SetIsCollected(IReadOnlyCollection<BrowseQuestionAppViewModel> list)
    {
        var ids = list.Select(x => x.Id).ToList();
        var userId = EngineContext.Current.ClaimManager.GetUserId().To<Guid>();
        var cacheKey = $"ids={string.Join(',', ids).ToMd5()}";
        var collection = await _cacheManager.GetAsync(
            QuestionCollectionCacheManager.ListQuery.Create(cacheKey), () => _repository.GetWhereAsync(x => x.CreatorId == userId && ids.Contains(x.QuestionId))
        );
        var dict = collection.ToDictionary(x => x.QuestionId, x => x.CreateTime);
        foreach (var item in list)
        {
            if (dict.TryGetValue(item.Id, out var value))
            {
                item.IsCollected = true;
                item.CollectionTime = value;
            }
        }
    }

    #endregion
}