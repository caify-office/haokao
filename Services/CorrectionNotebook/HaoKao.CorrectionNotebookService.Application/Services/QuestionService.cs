using HaoKao.CorrectionNotebookService.Application.Interfaces;
using HaoKao.CorrectionNotebookService.Application.Options;
using HaoKao.CorrectionNotebookService.Application.ViewModels;
using HaoKao.CorrectionNotebookService.Application.Workers;
using HaoKao.CorrectionNotebookService.Domain.Commands;
using HaoKao.CorrectionNotebookService.Domain.Entities;
using HaoKao.CorrectionNotebookService.Domain.Queries;
using HaoKao.CorrectionNotebookService.Domain.Repositories;
using HaoKao.CorrectionNotebookService.Domain.Services;
using HaoKao.CorrectionNotebookService.Domain.ValueObjects;

namespace HaoKao.CorrectionNotebookService.Application.Services;

/// <summary>
/// 题目服务接口
/// </summary>
/// <param name="bus"></param>
/// <param name="cacheManager"></param>
/// <param name="notifications"></param>
/// <param name="repository"></param>
/// <param name="queue"></param>
[DynamicWebApi]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class QuestionService(
    IMediatorHandler bus,
    IStaticCacheManager cacheManager,
    INotificationHandler<DomainNotification> notifications,
    IQuestionRepository repository,
    IQuestionQueue queue
) : IQuestionService
{
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly IStaticCacheManager _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
    private readonly DomainNotificationHandler _notifications = (DomainNotificationHandler)notifications;
    private readonly IQuestionRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly IQuestionQueue _queue = queue ?? throw new ArgumentNullException(nameof(queue));

    /// <summary>
    /// 获取题目列表
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<QuestionListViewModel> GetList([FromBody] QueryQuestionViewModel model)
    {
        var userId = EngineContext.Current.ClaimManager.GetUserId().To<Guid>();
        var query = new QuestionQuery(model.SubjectId, userId, model.MasteryDegree, model.CreateTime, model.TagIds, model.PageSize, model.PageIndex);
        var cacheKey = ServiceCacheKey<Question>.QueryCacheKey.Create(JsonSerializer.Serialize(query).ToMd5());
        var dto = await _cacheManager.GetAsync(cacheKey, () => _repository.GetPagedListAsync(query));
        var result = dto.Data.Select(x => new QuestionListItemViewModel(x.Id, x.ImageUrl, x.MasteryDegree, x.CreateTime)).ToList();
        return new QuestionListViewModel(dto.Count, result);
    }

    /// <summary>
    /// 获取题目详情
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id:guid}")]
    public async Task<QuestionViewModel> Get(Guid id)
    {
        var question = await _cacheManager.GetAsync(
            ServiceCacheKey<Question>.IdCacheKey.Create(id.ToString()),
            () => _repository.GetWithTagsAsync(id)
        );

        return new QuestionViewModel(
            question.Id,
            question.SubjectId,
            question.ImageUrl,
            question.Answer,
            question.Analysis,
            question.MasteryDegree,
            question.CreateTime,
            !question.Generatable,
            question.Tags.Select(x => new TagCategoryItemViewModel(x.Tag.Id, x.Tag.Name, x.Tag.IsBuiltIn, x.Tag.CreateTime)).ToList()
        );
    }

    /// <summary>
    /// 保存录入题目
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task Create([FromBody] CreateQuestionViewModel model)
    {
        var userId = EngineContext.Current.ClaimManager.GetUserId().To<Guid>();
        var command = new CreateQuestionCommand(model.SubjectId, userId, model.ImageUrls, model.TagIds);
        var questions = await _bus.TrySendCommand(command, _notifications);
        await Task.WhenAll(questions.Select(q => _queue.EnqueueAsync(q).AsTask()).ToArray());
        await _bus.RemoveCache(ServiceCacheKey<Question>.ListCacheKey);
        await _bus.RemoveCache(ServiceCacheKey<ExamLevel>.IdCacheKeyPrefix);
    }

    /// <summary>
    /// 生成题目答案与解析
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id:guid}")]
    public async Task<GetAnswerAndAnalysisViewModel> GetAnswerAndAnalysis(Guid id)
    {
        var question = await _repository.GetByIdAsync(id);
        if (question.Generatable == false)
        {
            return new GetAnswerAndAnalysisViewModel(question.Answer, question.Analysis);
        }

        var options = Singleton<AppSettings>.Instance.Get<EnabledServiceOptions>();
        var factory = EngineContext.Current.Resolve<IServiceFactory<ILargeLanguageModel>>();
        var llmId = options.PaidLLMService.Id;
        var llm = factory.Create(llmId);

        var path = await ImageService.DownloadImage(question.ImageUrl);
        await question.GenerateAnswerAndAnalysis(llm, path);
        question.Generatable = false;

        var command = new SaveAnswerAndAnalysisCommand(question);
        await _bus.TrySendCommand(command, _notifications);
        await _bus.RemoveCache(ServiceCacheKey<Question>.IdCacheKey.Create(id.ToString()));

        File.Delete(path);

        return new GetAnswerAndAnalysisViewModel(question.Answer, question.Analysis);
    }

    /// <summary>
    /// 生成题目答案与解析stream
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id:guid}")]
    public async IAsyncEnumerable<string> GetAnswerAndAnalysisStream(Guid id)
    {
        var question = await _repository.GetByIdAsync(id);
        if (question.Generatable == false)
        {
            yield break;
        }

        var options = Singleton<AppSettings>.Instance.Get<EnabledServiceOptions>();
        var factory = EngineContext.Current.Resolve<IServiceFactory<ILargeLanguageModel>>();
        var llmId = options.PaidLLMService.Id;
        var llm = factory.Create(llmId);

        var httpContext = EngineContext.Current.Resolve<IHttpContextAccessor>().HttpContext;
        httpContext.Response.ContentType = "text/event-stream";
        var path = await ImageService.DownloadImage(question.ImageUrl);
        await foreach (var chunk in question.GenerateAnswerAndAnalysisStream(llm, path))
        {
            try
            {
                await httpContext.Response.WriteAsync($"{chunk}\n");
            }
            catch (OperationCanceledException)
            {
                // 客户端断开连接
            }
        }

        question.Generatable = false;
        var command = new SaveAnswerAndAnalysisCommand(question);
        await _bus.TrySendCommand(command, _notifications);
        await _bus.RemoveCache(ServiceCacheKey<Question>.IdCacheKey.Create(id.ToString()));

        File.Delete(path);
    }

    /// <summary>
    /// 编辑掌握程度
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task MasteryDegree([FromBody] EditQuestionMasteryDegreeViewModel model)
    {
        var command = new EditQuestionMasteryDegreeCommand(model.Ids, model.MasteryDegree);
        await _bus.TrySendCommand(command, _notifications);
        await _bus.RemoveCache(ServiceCacheKey<Question>.ListCacheKey);
        await Task.WhenAll(model.Ids.Select(id => _bus.RemoveCache(ServiceCacheKey<Question>.IdCacheKey.Create(id.ToString()))).ToArray());
    }

    /// <summary>
    /// 编辑题目标签
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task QuestionTag([FromBody] EditQuestionTagViewModel model)
    {
        var command = new EditQuestionTagCommand(model.Id, model.TagIds);
        await _bus.TrySendCommand(command, _notifications);
        await _bus.RemoveCache(ServiceCacheKey<Question>.IdCacheKey.Create(model.Id.ToString()));
        await _bus.RemoveCache(ServiceCacheKey<Question>.ListCacheKey);
    }

    /// <summary>
    /// 删除题目
    /// </summary>
    /// <param name="subjectId"></param>
    /// <param name="ids"></param>
    /// <returns></returns>
    [HttpDelete("{subjectId:guid}")]
    public async Task Delete(Guid subjectId, [FromBody] IReadOnlyList<Guid> ids)
    {
        var command = new DeleteQuestionCommand(subjectId, ids);
        await _bus.TrySendCommand(command, _notifications);
        await _bus.RemoveCache(ServiceCacheKey<Question>.ListCacheKey);
        await _bus.RemoveCache(ServiceCacheKey<ExamLevel>.IdCacheKeyPrefix);
        await Task.WhenAll(ids.Select(id => _bus.RemoveCache(ServiceCacheKey<Question>.IdCacheKey.Create(id.ToString()))).ToArray());
    }

    /// <summary>
    /// 获取用户题目统计
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public Task<UserQuestionCountStatistics> GetUserQuestionCountStatistics()
    {
        var userId = EngineContext.Current.ClaimManager.GetUserId().To<Guid>();
        return _repository.GetQuestionCountStatisticsAsync(userId);
    }
}