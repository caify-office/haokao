using Girvs.Extensions;
using Girvs.Infrastructure;
using HaoKao.Common;
using HaoKao.LearningPlanService.Application.ViewModels.LearningTask;

namespace HaoKao.LearningPlanService.Application.Services.Web;

/// <summary>
/// 学习任务主类接口服务
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.WebSite)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class LearningTaskWebService : ILearningTaskWebService
{
    #region 初始参数

    private readonly IStaticCacheManager _cacheManager;
    private readonly IMediatorHandler _bus;
    private readonly DomainNotificationHandler _notifications;
    private readonly ILearningTaskRepository _repository;

    #endregion

    #region 构造函数

    /// <summary>
    /// 构造函数
    /// </summary>
    public LearningTaskWebService(
        IStaticCacheManager cacheManager,
        IMediatorHandler bus,
        INotificationHandler<DomainNotification> notifications,
        ILearningTaskRepository repository
    )
    {
        _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
        _bus = bus ?? throw new ArgumentNullException(nameof(bus));
        _notifications = (DomainNotificationHandler)notifications;
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    #endregion

    #region 服务方法

    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    [HttpGet("{id}")]
    public async Task<BrowseLearningTaskViewModel> Get(Guid id)
    {
        var learningTask = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<LearningTask>.ByIdCacheKey.Create(id.ToString()),
            async () => await _repository.GetByIdAsync(id)
        );

        if (learningTask == null) throw new GirvsException("对应的学习任务主类不存在", StatusCodes.Status404NotFound);

        return learningTask.MapToDto<BrowseLearningTaskViewModel>();
    }

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    [HttpGet]
    public async Task<LearningTaskQueryViewModel> Get([FromQuery] LearningTaskQueryViewModel queryViewModel)
    {
        var creatorId = EngineContext.Current.ClaimManager?.GetUserId()?.ToGuid();
        queryViewModel.CreatorId = creatorId;
        var query = queryViewModel.MapToQuery<LearningTaskQuery>();
        var tempQuery = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<LearningTask>.QueryCacheKey.Create(query.GetCacheKey()), async () =>
            {
                await _repository.GetByQueryAsync(query);
                return query;
            });

        if (!query.Equals(tempQuery))
        {
            query.RecordCount = tempQuery.RecordCount;
            query.Result = tempQuery.Result;
        }

        return query.MapToQueryDto<LearningTaskQueryViewModel, LearningTask>();
    }

    #endregion
}