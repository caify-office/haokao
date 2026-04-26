using HaoKao.StudentService.Application.Interfaces;
using HaoKao.StudentService.Application.ViewModels;
using HaoKao.StudentService.Domain.Commands;
using HaoKao.StudentService.Domain.Entities;
using HaoKao.StudentService.Domain.Repositories;

namespace HaoKao.StudentService.Application.Services;

/// <summary>
/// 学员参数设置接口服务-Web端
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.WebSite)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class StudentParameterConfigWebService(
    IMediatorHandler bus,
    INotificationHandler<DomainNotification> notifications,
    IStaticCacheManager cacheManager,
    IStudentParameterConfigRepository repository,
    IStudentParameterConfigService service
) : IStudentParameterConfigWebService
{
    #region 初始参数

    private readonly IStudentParameterConfigService _service = service ?? throw new ArgumentNullException(nameof(service));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly DomainNotificationHandler _notifications = (DomainNotificationHandler)notifications;
    private readonly IStaticCacheManager _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
    private readonly IStudentParameterConfigRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));

    private static CacheKey ByEffectiveNextDayCacheKey => new($"{nameof(StudentParameterConfig).ToLowerInvariant()}:effectivenextday:{DateTime.Now:yyyy-MM-dd}:{{0}}");

    #endregion

    #region 服务方法

    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    [HttpGet("{id}")]
    public Task<BrowseStudentParameterConfigViewModel> Get(Guid id)
    {
        return _service.Get(id);
    }

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    [HttpGet]
    public Task<QueryStudentParameterConfigViewModel> Get([FromQuery] QueryStudentParameterConfigViewModel queryViewModel)
    {
        return _service.Get(queryViewModel);
    }

    /// <summary>
    /// 保存学员参数设置
    /// </summary>
    /// <param name="model">新增模型</param>
    [HttpPost]
    public async Task Save([FromBody] CreateStudentParameterConfigViewModel model)
    {
        var command = model.MapToCommand<SaveStudentParameterConfigCommand>();

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 条件查询(隔日生效)
    /// </summary>
    /// <param name="model">查询对象</param>
    [HttpGet]
    public async Task<BrowseStudentParameterConfigViewModel> GetEffectiveNextDay([FromQuery] StudentParameterConfigQueryEffectiveNextDayViewModel model)
    {
        //获取当日才有效的缓存，如果没有取数据库最新数据
        var key = $"{model.UserId}-{model.PropertyType}-{model.PropertyName}".ToMd5();
        var getCurrentValue = await _cacheManager.GetAsync(
            ByEffectiveNextDayCacheKey.Create(key, cacheTime: (int)TimeSpan.FromDays(1).TotalMinutes),
            () => _repository.GetAsync(x => x.UserId == model.UserId && x.PropertyType == model.PropertyType && x.PropertyName == model.PropertyName)
        );

        return getCurrentValue.MapToDto<BrowseStudentParameterConfigViewModel>();
    }

    /// <summary>
    /// 保存学员参数设置(隔日生效)
    /// </summary>
    /// <param name="model">新增模型</param>
    [HttpPost]
    public async Task SaveEffectiveNextDay([FromBody] CreateStudentParameterConfigViewModel model)
    {
        //建立当日才有效的缓存
        var key = $"{model.UserId}-{model.PropertyType}-{model.PropertyName}".ToMd5();
        var getCurrentValue = await _cacheManager.GetAsync(
            ByEffectiveNextDayCacheKey.Create(key, cacheTime: (int)TimeSpan.FromDays(1).TotalMinutes),
            () => _repository.GetAsync(x => x.UserId == model.UserId && x.PropertyType == model.PropertyType && x.PropertyName == model.PropertyName)
        );
        //保存新的值
        await Save(model);
    }

    #endregion
}