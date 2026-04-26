using Girvs.AuthorizePermission;
using Girvs.AuthorizePermission.Enumerations;
using Girvs.Infrastructure;
using HaoKao.Common;
using HaoKao.PaperTempleteService.Application.ViewModels;
using HaoKao.PaperTempleteService.Domain.Commands;
using HaoKao.PaperTempleteService.Domain.Entities;
using HaoKao.PaperTempleteService.Domain.Queries;
using HaoKao.PaperTempleteService.Domain.Repositories;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;

namespace HaoKao.PaperTempleteService.Application.Services;

/// <summary>
/// 接口服务
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.Management)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsJwt)]
[ServicePermissionDescriptor(
    "试卷模板管理",
    "b83c7d73-f29e-ed5e-9d8e-4cd9fb2c62b5",
    "64",
    SystemModule.ExtendModule2,
    3
)]
public class PaperTempleteService(
    IStaticCacheManager cacheManager,
    IMediatorHandler bus,
    INotificationHandler<DomainNotification> notifications,
    IPaperTempleteRepository repository
) : IPaperTempleteService
{
    #region 初始参数

    private readonly IStaticCacheManager _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly DomainNotificationHandler _notifications = (DomainNotificationHandler)notifications;
    private readonly IPaperTempleteRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));

    #endregion

    #region 服务方法

    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    [HttpGet("{id:guid}")]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<BrowsePaperTempleteViewModel> Get(Guid id)
    {
        var entity = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<PaperTemplete>.ByIdCacheKey.Create(id.ToString()),
            () => _repository.GetByIdAsync(id)
        ) ?? throw new GirvsException("对应的不存在", StatusCodes.Status404NotFound);

        return entity.MapToDto<BrowsePaperTempleteViewModel>();
    }

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    [HttpGet]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<PaperTempleteQueryViewModel> Get([FromQuery] PaperTempleteQueryViewModel queryViewModel)
    {
        var query = queryViewModel.MapToQuery<PaperTempleteQuery>();
        var cacheKey = GirvsEntityCacheDefaults<PaperTemplete>.QueryCacheKey.Create(query.GetCacheKey());
        var tempQuery = await _cacheManager.GetAsync(cacheKey, async () =>
        {
            await _repository.GetByQueryAsync(query);
            return query;
        });

        if (!query.Equals(tempQuery))
        {
            query.RecordCount = tempQuery.RecordCount;
            query.Result = tempQuery.Result;
        }

        return query.MapToQueryDto<PaperTempleteQueryViewModel, PaperTemplete>();
    }

    /// <summary>
    /// 根据查询获取列表
    /// </summary>
    /// <param name="subjectId">科目Id</param>
    [HttpGet]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<IReadOnlyList<PaperTemplete>> GetListBySubjectId(string subjectId)
    {
        var key = GirvsEntityCacheDefaults<PaperTemplete>.QueryCacheKey.Create($"subjectId={subjectId}");
        return await _cacheManager.GetAsync(key, () => _repository.GetPaperTempleteByQueryAsync(subjectId));
    }

    /// <summary>
    /// 创建
    /// </summary>
    /// <param name="model">新增模型</param>
    [HttpPost]
    [ServiceMethodPermissionDescriptor("新增", Permission.Post, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task Create([FromBody] CreatePaperTempleteViewModel model)
    {
        var command = new CreatePaperTempleteCommand(
            model.TempleteName,
            model.Remark,
            model.SuitableSubjects
        );

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 根据主键删除指定
    /// </summary>
    /// <param name="id">主键</param>
    [HttpDelete("{id:guid}")]
    [ServiceMethodPermissionDescriptor("删除", Permission.Delete, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task Delete(Guid id)
    {
        var command = new DeletePaperTempleteCommand(id);

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 根据主键更新指定
    /// </summary>
    /// <param name="id">主键</param>
    /// <param name="model">新增模型</param>
    [HttpPut("{id:guid}")]
    [ServiceMethodPermissionDescriptor("修改", Permission.Edit, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task Update(Guid id, [FromBody] UpdatePaperTempleteViewModel model)
    {
        var command = new UpdatePaperTempleteCommand(
            id,
            model.TempleteName,
            model.Remark,
            model.SuitableSubjects
        );

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 根据主键更新模板结构
    /// </summary>
    /// <param name="id">主键</param>
    /// <param name="model">新增模型</param>
    [HttpPut("{id:guid}")]
    [ServiceMethodPermissionDescriptor("设置模板结构", Permission.Edit_Extend1, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task UpdateStruct(Guid id, [FromBody] UpdateTempleteStructViewModel model)
    {
        var command = new UpdateTempleteStructCommand(id, model.TempleteStructDatas);

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    #endregion
}