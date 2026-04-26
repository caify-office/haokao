using HaoKao.CorrectionNotebookService.Application.Interfaces;
using HaoKao.CorrectionNotebookService.Application.ViewModels;
using HaoKao.CorrectionNotebookService.Domain.Commands;
using HaoKao.CorrectionNotebookService.Domain.Entities;
using HaoKao.CorrectionNotebookService.Domain.Repositories;

namespace HaoKao.CorrectionNotebookService.Application.Services;

/// <summary>
/// 科目接口
/// </summary>
/// <param name="bus"></param>
/// <param name="cacheManager"></param>
/// <param name="notifications"></param>
/// <param name="repository"></param>
[DynamicWebApi]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class SubjectService(
    IMediatorHandler bus,
    IStaticCacheManager cacheManager,
    INotificationHandler<DomainNotification> notifications,
    IExamLevelRepository repository
) : ISubjectService
{
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly IStaticCacheManager _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
    private readonly DomainNotificationHandler _notifications = (DomainNotificationHandler)notifications;
    private readonly IExamLevelRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));

    /// <summary>
    /// 根据考试级别ID获取科目列表
    /// </summary>
    /// <param name="examLevelId">考试级别ID</param>
    /// <returns>科目列表</returns>
    [HttpGet("{examLevelId:guid}"), AllowAnonymous]
    public async Task<IReadOnlyList<SubjectListItemViewModel>> Get(Guid examLevelId)
    {
        var userId = EngineContext.Current.IsAuthenticated ? EngineContext.Current.ClaimManager.GetUserId().To<Guid>() : Guid.Empty;
        var examLevel = await _cacheManager.GetAsync(
            ServiceCacheKey<ExamLevel>.IdCacheKey.Create(examLevelId.ToString()),
            () => _repository.GetWithSubjectByUserAsync(examLevelId, userId)
        );
        // var examLevel = await _repository.GetWithSubjectByUserAsync(examLevelId, userId);
        return examLevel.Subjects.Select(x => new SubjectListItemViewModel(
            x.Id, x.Name, x.QuestionCount, x.IsBuiltIn, x.Sort.Priority, x.Icon
        )).ToList();
    }

    /// <summary>
    /// 创建科目
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task Create([FromBody] CreateSubjectViewModel model)
    {
        var userId = EngineContext.Current.ClaimManager.GetUserId().To<Guid>();
        var command = new CreateSubjectCommand(model.Name, model.ExamLevelId, userId);
        await _bus.TrySendCommand(command, _notifications);
        await _bus.RemoveCache(ServiceCacheKey<ExamLevel>.IdCacheKey.Create(model.ExamLevelId.ToString()));
    }

    /// <summary>
    /// 重新排序科目
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task Resort([FromBody] ResortSubjectViewModel model)
    {
        var userId = EngineContext.Current.ClaimManager.GetUserId().To<Guid>();
        var sorts = model.Dict.Select(x => SubjectSort.Create(userId, x.Key, x.Value)).ToList();
        var command = new ResortSubjectCommand(model.ExamLevelId, userId, sorts);
        await _bus.TrySendCommand(command, _notifications);
        await _bus.RemoveCache(ServiceCacheKey<ExamLevel>.IdCacheKey.Create(model.ExamLevelId.ToString()));
    }

    /// <summary>
    /// 删除科目
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id:guid}")]
    public async Task Delete(Guid id)
    {
        var command = new DeleteSubjectCommand(id);
        var examLevelId = await _bus.TrySendCommand(command, _notifications);
        var cacheKey = ServiceCacheKey<ExamLevel>.IdCacheKey.Create(examLevelId.ToString());
        await _bus.RemoveCache(cacheKey.Create());
    }
}