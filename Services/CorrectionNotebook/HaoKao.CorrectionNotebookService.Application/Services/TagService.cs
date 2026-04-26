using HaoKao.CorrectionNotebookService.Application.Interfaces;
using HaoKao.CorrectionNotebookService.Application.ViewModels;
using HaoKao.CorrectionNotebookService.Domain.Commands;
using HaoKao.CorrectionNotebookService.Domain.Entities;
using HaoKao.CorrectionNotebookService.Domain.Repositories;

namespace HaoKao.CorrectionNotebookService.Application.Services;


/// <summary>
/// 标签接口
/// </summary>
/// <param name="bus"></param>
/// <param name="cacheManager"></param>
/// <param name="notifications"></param>
/// <param name="repository"></param>
[DynamicWebApi]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class TagService(
    IMediatorHandler bus,
    IStaticCacheManager cacheManager,
    INotificationHandler<DomainNotification> notifications,
    ITagRepository repository
) : ITagService
{
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly IStaticCacheManager _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
    private readonly DomainNotificationHandler _notifications = (DomainNotificationHandler)notifications;
    private readonly ITagRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));

    /// <summary>
    /// 获取标签列表
    /// </summary>
    /// <returns>标签列表</returns>
    [HttpGet]
    public async Task<IReadOnlyList<TagCategoryViewModel>> Get()
    {
        var userId = EngineContext.Current.ClaimManager.GetUserId().To<Guid>();
        var tags = await _cacheManager.GetAsync(ServiceCacheKey<Tag>.ListCacheKey, () => _repository.GetByUserAsync(userId));
        var list = tags.GroupBy(t => t.Category)
                   .Select(x => new TagCategoryViewModel(
                                    x.Key, x.Min(g => g.IsBuiltIn), x.Min(g => g.CreateTime), x.Select(g => new TagCategoryItemViewModel(
                                        g.Id, g.Name, g.IsBuiltIn, g.CreateTime
                                    )).ToList())
                          ).ToList();

        // 没有自定义标签需要添加自定义标签分类
        if (!list.Any(x => x.Category == "自定义标签"))
        {
            list.Add(new TagCategoryViewModel("自定义标签", false, DateTime.Now, []));
        }

        return list;
    }

    /// <summary>
    /// 创建标签
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task Create([FromBody] CreateTagViewModel model)
    {
        var userId = EngineContext.Current.ClaimManager.GetUserId().To<Guid>();
        var command = new CreateTagCommand(model.Name, userId);
        await _bus.TrySendCommand(command, _notifications);
        await _bus.RemoveCache(ServiceCacheKey<Tag>.ListCacheKey);
    }

    /// <summary>
    /// 删除标签
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id:guid}")]
    public async Task Delete(Guid id)
    {
        var command = new DeleteTagCommand(id);
        await _bus.TrySendCommand(command, _notifications);
        await _bus.RemoveCache(ServiceCacheKey<Tag>.ListCacheKey);
        await _bus.RemoveCache(ServiceCacheKey<Question>.IdCacheKeyPrefix);
    }
}