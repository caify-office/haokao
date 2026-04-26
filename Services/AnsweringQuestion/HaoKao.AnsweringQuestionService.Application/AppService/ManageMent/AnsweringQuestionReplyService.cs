using Girvs.AuthorizePermission.Enumerations;
using Girvs.Infrastructure;
using HaoKao.AnsweringQuestionService.Application.ViewModels;
using HaoKao.AnsweringQuestionService.Application.ViewModels.AnsweringQuestionReply;
using HaoKao.AnsweringQuestionService.Domain.Commands.AnsweringQuestionReply;
using HaoKao.AnsweringQuestionService.Domain.Entities;
using HaoKao.AnsweringQuestionService.Domain.Queries.EntityQuery;
using HaoKao.AnsweringQuestionService.Domain.Repositories;
using HaoKao.Common;
using Microsoft.AspNetCore.Authorization;

namespace HaoKao.AnsweringQuestionService.Application.AppService.ManageMent;

/// <summary>
/// 答疑回复接口服务
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.Management)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsJwt)]
[ServicePermissionDescriptor(
    "答疑回复接口服务",
    "f49592e9-da6a-4105-817a-aca91ebe8e4c",
    "512",
    SystemModule.ExtendModule2 | SystemModule.ExtendModule3,
    3
)]
public class AnsweringQuestionReplyService(
    IStaticCacheManager cacheManager,
    IMediatorHandler bus,
    INotificationHandler<DomainNotification> notifications,
    IAnsweringQuestionReplyRepository repository
) : IAnsweringQuestionReplyService
{
    #region 初始参数

    private readonly IStaticCacheManager _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly DomainNotificationHandler _notifications = (DomainNotificationHandler)notifications;
    private readonly IAnsweringQuestionReplyRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));

    #endregion

    #region 服务方法

    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    [HttpGet("{id}")]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<BrowseAnsweringQuestionReplyViewModel> Get(Guid id)
    {
        var answeringQuestionReply = await _cacheManager.GetAsync(
              GirvsEntityCacheDefaults<AnsweringQuestionReply>.ByIdCacheKey.Create(id.ToString()),
              () => _repository.GetByIdAsync(id)
        ) ?? throw new GirvsException("对应的答疑回复不存在", StatusCodes.Status404NotFound);

        return answeringQuestionReply.MapToDto<BrowseAnsweringQuestionReplyViewModel>();
    }

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    [HttpGet]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<AnsweringQuestionReplyQueryViewModel> Get([FromQuery] AnsweringQuestionReplyQueryViewModel queryViewModel)
    {
        var query = queryViewModel.MapToQuery<AnsweringQuestionReplyQuery>();
        await _repository.GetByQueryAsync(query);
        return query.MapToQueryDto<AnsweringQuestionReplyQueryViewModel, AnsweringQuestionReply>();
    }

    /// <summary>
    /// 创建答疑回复
    /// </summary>
    /// <param name="model">新增模型</param>
    [HttpPost]
    [ServiceMethodPermissionDescriptor("创建", Permission.Post, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task Create([FromBody] CreateAnsweringQuestionReplyViewModel model)
    {
        var command = new CreateAnsweringQuestionReplyCommand(
            model.ReplyContent,
            model.AnsweringQuestionId
        );

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 根据主键删除指定答疑回复
    /// </summary>
    /// <param name="id">主键</param>
    [HttpDelete("{id}")]
    [ServiceMethodPermissionDescriptor("删除", Permission.Delete, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task Delete(Guid id)
    {
        var command = new DeleteAnsweringQuestionReplyCommand(id);

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 根据主键更新指定答疑回复
    /// </summary>
    /// <param name="id">主键</param>
    /// <param name="model">新增模型</param>
    [HttpPut("{id}")]
    [ServiceMethodPermissionDescriptor("编辑", Permission.Edit, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task Update(Guid id, [FromBody] UpdateAnsweringQuestionReplyViewModel model)
    {
        var command = new UpdateAnsweringQuestionReplyCommand(
            id,
            model.ReplyContent,
            model.AnsweringQuestionId
        );

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    #endregion
}