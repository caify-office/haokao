using Girvs.AuthorizePermission;
using Girvs.AuthorizePermission.Enumerations;
using Girvs.Driven.Extensions;
using Girvs.Extensions;
using Girvs.Infrastructure;
using HaoKao.Common;
using HaoKao.LecturerService.Application.Interfaces;
using HaoKao.LecturerService.Application.ViewModels;
using HaoKao.LecturerService.Domain.Commands;
using HaoKao.LecturerService.Domain.Entities;
using HaoKao.LecturerService.Domain.Queries;
using HaoKao.LecturerService.Domain.Repositories;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Linq;

namespace HaoKao.LecturerService.Application.Services;

/// <summary>
/// 讲师接口服务-管理端
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.Management)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsJwt)]
[ServicePermissionDescriptor(
    "主讲老师",
    "0f8b126a-309f-4aa4-b1fb-fe1471e803e0",
    "128",
    SystemModule.ExtendModule2 | SystemModule.ExtendModule3,
    3
)]
public class LecturerService(
    IStaticCacheManager cacheManager,
    IMediatorHandler bus,
    INotificationHandler<DomainNotification> notifications,
    ILecturerRepository repository
) : ILecturerService
{
    #region 初始参数

    private readonly IStaticCacheManager _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly DomainNotificationHandler _notifications = (DomainNotificationHandler)notifications;
    private readonly ILecturerRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));

    #endregion

    #region 服务方法

    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    [HttpGet("{id:guid}")]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View,
                                       UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<BrowseLecturerViewModel> Get(Guid id)
    {
        var lecturer = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<Lecturer>.ByIdCacheKey.Create(id.ToString()),
            () => _repository.GetByIdAsync(id)
        ) ?? throw new GirvsException("对应的讲师不存在", StatusCodes.Status404NotFound);

        return lecturer.MapToDto<BrowseLecturerViewModel>();
    }

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    [HttpGet]
    public async Task<LecturerQueryViewModel> Get([FromQuery] LecturerQueryViewModel queryViewModel)
    {
        var query = queryViewModel.MapToQuery<LecturerQuery>();
        var tempQuery = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<Lecturer>.QueryCacheKey.Create(query.GetCacheKey()), async () =>
            {
                await _repository.GetByQueryAsync(query);
                return query;
            });

        if (!query.Equals(tempQuery))
        {
            query.RecordCount = tempQuery.RecordCount;
            query.Result = tempQuery.Result;
        }

        return query.MapToQueryDto<LecturerQueryViewModel, Lecturer>();
    }

    /// <summary>
    /// 根据主键数组获取讲师列表
    /// </summary>
    /// <param name="ids">主键</param>
    [HttpPost]
    public async Task<IReadOnlyList<BrowseLecturerViewModel>> GetByIds([FromBody] Guid[] ids)
    {
        var key = string.Join(",", ids.ToList().OrderBy(x => x)).ToMd5();
        var list = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<Lecturer>.ByIdsCacheKey.Create(key),
            () => _repository.GetWhereAsync(x => ids.Contains(x.Id))
        );
        return list.MapTo<List<BrowseLecturerViewModel>>();
    }

    /// <summary>
    /// 创建讲师
    /// </summary>
    /// <param name="model">新增模型</param>
    [HttpPost]
    [ServiceMethodPermissionDescriptor("新增", Permission.Post, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task Create([FromBody] CreateLecturerViewModel model)
    {
        var command = model.MapToCommand<CreateLecturerCommand>();

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 根据主键更新指定讲师
    /// </summary>
    /// <param name="model">新增模型</param>
    [HttpPut]
    [ServiceMethodPermissionDescriptor("编辑", Permission.Edit, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task Update([FromBody] UpdateLecturerViewModel model)
    {
        var command = model.MapToCommand<UpdateLecturerCommand>();

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 根据主键删除指定讲师
    /// </summary>
    /// <param name="id">主键</param>
    [HttpDelete("{id:guid}")]
    [ServiceMethodPermissionDescriptor("删除", Permission.Delete, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task Delete(Guid id)
    {
        var command = new DeleteLecturerCommand(id);

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    #endregion
}