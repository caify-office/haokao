using Girvs.Driven.Extensions;
using Girvs.Extensions;
using Girvs.Infrastructure;
using HaoKao.Common;
using HaoKao.Common.CacheManager;
using HaoKao.LearningPlanService.Application.ViewModels.LearningPlan;
using HaoKao.LearningPlanService.Domain.Records;
using HaoKao.LearningPlanService.Domain.RemoteRepositories;
using System.Linq;

namespace HaoKao.LearningPlanService.Application.Services.Web;

/// <summary>
/// 学习计划主类，用于组织和管理一系列学习任务接口服务
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.WebSite)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class LearningPlanWebService(
    IStaticCacheManager cacheManager,
    IMediatorHandler bus,
    INotificationHandler<DomainNotification> notifications,
    ILearningPlanRepository repository,
    IRemoteCourseRepository remoteCourseRepository,
    IRemoteQuestionRepository remoteQuestionRepository
) : ILearningPlanWebService
{
    private readonly IStaticCacheManager _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly DomainNotificationHandler _notifications = (DomainNotificationHandler)notifications;
    private readonly ILearningPlanRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));

    #region 服务方法

    /// <summary>
    /// 获取当前用户学习计划
    /// </summary>
    [HttpGet("{productId:guid}/{subjectId:guid}")]
    public async Task<BrowseLearningPlanViewModel> GetCurrentLearningPlan(Guid productId, Guid subjectId)
    {
        var userId = EngineContext.Current.ClaimManager.GetUserId().ToGuid();
        var keyStr = $"{userId}_{subjectId}_{productId}".ToMd5();
        var learningPlan = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<LearningPlan>.QueryCacheKey.Create(keyStr),
            () => _repository.GetIncludeByCreatorIdAsync(userId, subjectId, productId)
        );

        if (learningPlan == null) return new BrowseLearningPlanViewModel();

        return learningPlan.MapToDto<BrowseLearningPlanViewModel>();
    }

    /// <summary>
    /// 获取任务总数和总时长
    /// </summary>
    /// <param name="model">新增模型</param>
    [HttpPost("QueryTaskCountAndDurationsViewModel")]
    public async Task<StatisticsTaskCountDurationsViewModel> Get([FromBody] QueryTaskCountAndDurationsViewModel model)
    {
        var tenantId = EngineContext.Current.ClaimManager.GetTenantId();
        var cacheKey = StatisticsAssistantProductPermissionManager.BuildCacheKey(tenantId, model.ProductId, model.SubjectId).Create();
        var result = await GetKnowledgePointTaskCountDurations(model.KnowledgePointTasks);
        var cacheResult = await _cacheManager.GetAsync(cacheKey, async () => await Task.FromResult(new StatisticsTaskCountDurationsViewModel()));
        result.TaskCount += cacheResult.TaskCount;
        result.TaskTotalDurations += cacheResult.TaskTotalDurations;
        return result;
    }

    private async Task<StatisticsTaskCountDurationsViewModel> GetKnowledgePointTaskCountDurations(IEnumerable<KnowledgePointTask> tasks)
    {
        var model = new StatisticsTaskCountDurationsViewModel();
        foreach (var task in tasks)
        {
            model.TaskCount += task.VideoInfoViewModels.Count(x => x.VideoDurationSeconds > 0);
            model.TaskTotalDurations += (int)task.VideoInfoViewModels.Sum(x => x.VideoDurationSeconds) / 60;

            // 整理智辅课程章节练习任务，放入章节练习任务组中（先通过课程id和章节Id）
            var chapterPractice = await remoteCourseRepository.GetChapterPractice(task.CourseId, task.ChapterId);
            if (chapterPractice?.ChapterNodeId == null || !chapterPractice.QuestionCategoryId.HasValue) continue;

            // 通过课程id和章节id获取配置试题章节id和试题分类id
            var count = await remoteQuestionRepository.GetChapterCategorieQuestionCount(chapterPractice.ChapterNodeId.Value, chapterPractice.QuestionCategoryId.Value);
            if (count == 0) continue;

            model.TaskCount += 1;
            model.TaskTotalDurations += count; // 1道题算1分钟
        }
        return model;
    }

    /// <summary>
    /// 创建学习计划主类，用于组织和管理一系列学习任务
    /// </summary>
    /// <param name="model">新增模型</param>
    [HttpPost]
    public async Task Create([FromBody] CreateLearningPlanViewModel model)
    {
        const int minutes = 60;
        if (!model.DayLearningTimes.Exists(x => x >= minutes))
        {
            throw new GirvsException(StatusCodes.Status400BadRequest, $"每日学习时长不能都小于{minutes}分钟");
        }
        var command = model.MapToCommand<CreateLearningPlanCommand>();

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 根据主键删除指定学习计划主类，用于组织和管理一系列学习任务
    /// </summary>
    /// <param name="id">主键</param>
    [HttpDelete("{id:guid}")]
    public async Task Delete(Guid id)
    {
        var command = new DeleteLearningPlanCommand(id);

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    #endregion
}