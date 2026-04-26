using HaoKao.CorrectionNotebookService.Application.Interfaces;
using HaoKao.CorrectionNotebookService.Application.ViewModels;
using HaoKao.CorrectionNotebookService.Domain.Commands;
using HaoKao.CorrectionNotebookService.Domain.Entities;
using HaoKao.CorrectionNotebookService.Domain.Queries;
using HaoKao.CorrectionNotebookService.Domain.Repositories;

namespace HaoKao.CorrectionNotebookService.Application.Services;


/// <summary>
/// 考试级别接口
/// </summary>
/// <param name="bus"></param>
/// <param name="cacheManager"></param>
/// <param name="notifications"></param>
/// <param name="repository"></param>
[DynamicWebApi]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class ExamLevelService(
    IMediatorHandler bus,
    IStaticCacheManager cacheManager,
    INotificationHandler<DomainNotification> notifications,
    IExamLevelRepository repository
) : IExamLevelService
{
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly IStaticCacheManager _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
    private readonly DomainNotificationHandler _notifications = (DomainNotificationHandler)notifications;
    private readonly IExamLevelRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));

    /// <summary>
    /// 获取所有考试级别
    /// </summary>
    /// <returns></returns>
    [HttpGet, AllowAnonymous]
    public async Task<IReadOnlyList<ExamLevelListItemViewModel>> Get()
    {
        Guid? userId = EngineContext.Current.IsAuthenticated ? EngineContext.Current.ClaimManager.GetUserId().To<Guid>() : null;

        var query = new ExamLevelQuery(userId);
        var cacheKey = ServiceCacheKey<ExamLevel>.ListCacheKey;
        var examLevels = await _cacheManager.GetAsync(cacheKey, () => _repository.GetListByUserAsync(query));

        return examLevels.Where(x => x.ParentId == Guid.Empty).Select(x =>
            new ExamLevelListItemViewModel(
                x.Id, x.Name, x.IsBuiltIn, x.CreateTime,
                examLevels.Where(c => x.Id == c.ParentId)
                          .Select(c => new ExamLevelListItemChild(c.Id, c.Name, c.IsBuiltIn, c.ParentId, c.CreateTime))
                          .ToList()
        )).ToList();
    }

    /// <summary>
    /// 创建考试级别
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task Create([FromBody] CreateExamLevelViewModel model)
    {
        var userId = EngineContext.Current.ClaimManager.GetUserId().To<Guid>();
        var command = new CreateExamLevelCommand(model.Name, model.ParentId, userId);
        await _bus.TrySendCommand(command, _notifications);
        await _bus.RemoveCache(ServiceCacheKey<ExamLevel>.ListCacheKey);
    }

    /// <summary>
    /// 编辑考试级别名称
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task EditName([FromBody] EditExamLevelNameViewModel model)
    {
        var command = new EditExamLevelNameCommand(model.Id, model.Name);
        await _bus.TrySendCommand(command, _notifications);
        await _bus.RemoveCache(ServiceCacheKey<ExamLevel>.ListCacheKey);
    }

    /// <summary>
    /// 删除考试级别
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id:guid}")]
    public async Task Delete(Guid id)
    {
        var userId = EngineContext.Current.ClaimManager.GetUserId().To<Guid>();
        var command = new DeleteExamLevelCommand(id, userId);
        await _bus.TrySendCommand(command, _notifications);
        await _bus.RemoveCache(ServiceCacheKey<ExamLevel>.ListCacheKey);
    }
}