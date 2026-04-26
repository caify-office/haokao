using HaoKao.QuestionService.Application.QuestionCollectionModule.Interfaces;
using HaoKao.QuestionService.Application.QuestionCollectionModule.ViewModels;
using HaoKao.QuestionService.Application.QuestionHandlers;
using HaoKao.QuestionService.Domain.CacheExtensions;
using HaoKao.QuestionService.Domain.QuestionCollectionModule;
using HaoKao.QuestionService.Domain.QuestionModule;

namespace HaoKao.QuestionService.Application.QuestionCollectionModule.Services;

public class QuestionCollectionService(
    IMediatorHandler bus,
    IStaticCacheManager cacheManager,
    INotificationHandler<DomainNotification> notifications,
    IQuestionCollectionRepository repository
) : IQuestionCollectionService
{
    #region 初始参数

    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly IStaticCacheManager _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
    private readonly DomainNotificationHandler _notifications = (DomainNotificationHandler)notifications;
    private readonly IQuestionCollectionRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));

    #endregion

    #region 服务方法

    /// <summary>
    /// 获取当前用户试题收藏分类统计
    /// </summary>
    /// <returns></returns>
    public Task<List<QuestionCollectionStatViewModel>> Get(Guid subjectId)
    {
        return _cacheManager.GetAsync(
            QuestionCollectionCacheManager.ListQuery.Create($"{nameof(Get)}:subjectId={subjectId}"),
            () => GetQuestionCollectionStateList(subjectId)
        );
    }

    private async Task<List<QuestionCollectionStatViewModel>> GetQuestionCollectionStateList(Guid subjectId)
    {
        var questions = EngineContext.Current.Resolve<IEnumerable<IQuestion>>();
        var questionTypes = questions.OrderBy(x => x.Code).ToDictionary(x => x.QuestionTypeId, x => x.QuestionTypeName);
        var collectionQuestionTypes = await _repository.GetCollectionQuestionTypes(subjectId);
        var list = questionTypes.Select(t => new QuestionCollectionStatViewModel
        {
            TypeId = t.Key.ToString(),
            TypeName = t.Value,
            Count = CalcCount(t.Key.ToString()),
        }).Where(x => x.Count > 0).ToList();
        return list;

        int CalcCount(string typeId)
        {
            // 有父Id的 + 无父Id的
            var count1 = collectionQuestionTypes.Where(x => string.IsNullOrEmpty(x.ParentTypeId)).Count(x => x.TypeId == typeId);
            var count2 = collectionQuestionTypes.Where(x => !string.IsNullOrEmpty(x.ParentTypeId)).Count(x => x.ParentTypeId == typeId);
            return count1 + count2;
        }
    }

    /// <summary>
    /// 获取收藏的试题列表，带分页、带条件查询
    /// </summary>
    /// <param name="viewModel"></param>
    /// <returns></returns>
    public async Task<QueryQuestionCollectionViewModel> Get(QueryQuestionCollectionViewModel viewModel)
    {
        var query = viewModel.MapToQuery<QuestionCollectionQuery>();
        var tempQuery = await _cacheManager.GetAsync(QuestionCollectionCacheManager.ListQuery.Create(query.GetCacheKey()), async () =>
        {
            await _repository.GetByQueryAsync(query);
            return query;
        });
        if (!query.Equals(tempQuery))
        {
            query.RecordCount = tempQuery.RecordCount;
            query.Result = tempQuery.Result;
        }

        // await _repository.GetByQueryAsync(query);

        // 过滤掉被禁用的题目
        query.Result = query.Result.Where(x => x.Question.EnableState == EnableState.Enable).ToList();

        return query.MapToQueryDto<QueryQuestionCollectionViewModel, QuestionCollection>();
    }

    /// <summary>
    /// 是否收藏
    /// </summary>
    /// <param name="questionId"></param>
    /// <returns></returns>
    public async Task<bool> Any(Guid questionId)
    {
        var result = await _cacheManager.GetAsync(
            QuestionCollectionCacheManager.ListQuery.Create($"{nameof(Any)}:questionId={questionId}"),
            () => _repository.IsCollected(questionId)
        );
        return result;
    }

    /// <summary>
    /// 创建试题收藏
    /// </summary>
    /// <param name="questionId">试题Id</param>
    public async Task Create(Guid questionId)
    {
        var command = new CreateQuestionCollectionCommand(questionId);

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 根据试题Id删除指定试题收藏
    /// </summary>
    /// <param name="questionId">试题Id</param>
    public async Task Delete(Guid questionId)
    {
        var command = new DeleteQuestionCollectionCommand(questionId);

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    #endregion
}