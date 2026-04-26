using Girvs.Driven.CacheDriven.Events;
using HaoKao.StudentService.Application.Interfaces;
using HaoKao.StudentService.Application.ViewModels;
using HaoKao.StudentService.Domain.Commands;
using HaoKao.StudentService.Domain.Entities;
using HaoKao.StudentService.Domain.Repositories;

namespace HaoKao.StudentService.Application.Services;

/// <summary>
/// 学员接口服务--Web端
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.WebSite)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class StudentWebService(
    IStaticCacheManager cacheManager,
    IMediatorHandler bus,
    INotificationHandler<DomainNotification> notifications,
    IStudentRepository repository
) : IStudentWebService
{
    #region 初始参数

    private readonly IStaticCacheManager _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly DomainNotificationHandler _notifications = (DomainNotificationHandler)notifications;
    private readonly IStudentRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));

    #endregion

    #region 服务方法

    /// <summary>
    /// 获取当前租户下的学员信息
    /// </summary>
    [HttpGet]
    public async Task<BrowseStudentViewModel> Get()
    {
        var result = await GetCurrent();
        if (result == null)
        {
            await Create();
            return await GetCurrent();
        }
        return result;
    }

    private async Task<BrowseStudentViewModel> GetCurrent()
    {
        var userId = EngineContext.Current.ClaimManager.GetUserId().To<Guid>();
        var student = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<Student>.ByIdCacheKey.Create(userId.ToString()),
            () => _repository.GetIncludeAsync(x => x.RegisterUserId == userId)
        );
        return student?.MapToDto<BrowseStudentViewModel>();
    }

    /// <summary>
    /// 创建学员
    /// </summary>
    private async Task Create()
    {
        var userId = EngineContext.Current.ClaimManager.GetUserId().To<Guid>();

        var command = new CreateStudentCommand(userId);

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }

        var cacheKey = GirvsEntityCacheDefaults<Student>.ByIdCacheKey.Create(userId.ToString());
        await _bus.RaiseEvent(new RemoveCacheEvent(cacheKey));
    }

    #endregion
}