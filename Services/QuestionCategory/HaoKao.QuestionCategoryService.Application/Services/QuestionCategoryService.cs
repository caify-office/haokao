using Girvs.AuthorizePermission.Enumerations;
using Girvs.Driven.Extensions;
using Girvs.Infrastructure;
using HaoKao.Common;
using HaoKao.QuestionCategoryService.Application.Interfaces;
using HaoKao.QuestionCategoryService.Application.ViewModels;
using HaoKao.QuestionCategoryService.Domain.Commands;
using HaoKao.QuestionCategoryService.Domain.Entities;
using HaoKao.QuestionCategoryService.Domain.Queries;
using HaoKao.QuestionCategoryService.Domain.Repositories;
using Microsoft.AspNetCore.Authorization;
using System.Linq;

namespace HaoKao.QuestionCategoryService.Application.Services;

/// <summary>
/// 题库类别接口服务
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.Management)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsJwt)]
[ServicePermissionDescriptor(
    "题库类别",
    "7913fe74-cb87-4a0f-9b38-c5a0619078cd",
    "64",
    SystemModule.ExtendModule2 | SystemModule.ExtendModule3,
    3
)]
public class QuestionCategoryService(
    IStaticCacheManager cacheManager,
    IMediatorHandler bus,
    IMapper mapper,
    INotificationHandler<DomainNotification> notifications,
    IQuestionCategoryRepository repository
) : IQuestionCategoryService
{
    #region 初始参数

    private readonly IStaticCacheManager _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    private readonly DomainNotificationHandler _notifications = (DomainNotificationHandler)notifications;
    private readonly IQuestionCategoryRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));

    #endregion

    #region 服务方法

    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    [HttpGet("{id:guid}")]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<BrowseQuestionCategoryViewModel> Get(Guid id)
    {
        var questionCategory = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<QuestionCategory>.ByIdCacheKey.Create(id.ToString()),
            () => _repository.GetByIdAsync(id)
        ) ?? throw new GirvsException("对应的题库类别不存在", StatusCodes.Status404NotFound);
        return questionCategory.MapToDto<BrowseQuestionCategoryViewModel>();
    }

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    [HttpGet]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<QuestionCategoryQueryViewModel> Get([FromQuery] QuestionCategoryQueryViewModel queryViewModel)
    {
        var query = queryViewModel.MapToQuery<QuestionCategoryQuery>();
        query.OrderBy = nameof(QuestionCategory.CreateTime);
        var tempQuery = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<QuestionCategory>.QueryCacheKey.Create(query.GetCacheKey()),
            async () =>
            {
                await _repository.GetByQueryAsync(query);
                return query;
            }
        );
        if (!query.Equals(tempQuery))
        {
            query.RecordCount = tempQuery.RecordCount;
            query.Result = tempQuery.Result;
        }

        return query.MapToQueryDto<QuestionCategoryQueryViewModel, QuestionCategory>();
    }

    /// <summary>
    /// 获取类别列表
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<List<BrowseQuestionCategoryViewModel>> GetCategoryList()
    {
        var list = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<QuestionCategory>.QueryCacheKey.Create(nameof(GetCategoryList)),
            () => _repository.Query.OrderBy(x => x.CreateTime).ToListAsync()
        );
        return _mapper.Map<List<BrowseQuestionCategoryViewModel>>(list);
    }

    /// <summary>
    /// 创建题库类别
    /// </summary>
    /// <param name="model">新增模型</param>
    [HttpPost]
    [ServiceMethodPermissionDescriptor("新增", Permission.Post, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task Create([FromBody] CreateQuestionCategoryViewModel model)
    {
        var command = model.MapToCommand<CreateQuestionCategoryCommand>();

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 根据主键删除指定题库类别
    /// </summary>
    /// <param name="id">主键</param>
    [HttpDelete("{id:guid}")]
    [ServiceMethodPermissionDescriptor("删除", Permission.Delete, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task Delete(Guid id)
    {
        var command = new DeleteQuestionCategoryCommand(id);

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 根据主键更新指定题库类别
    /// </summary>
    /// <param name="model">新增模型</param>
    [HttpPut]
    [ServiceMethodPermissionDescriptor("编辑", Permission.Edit, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task Update([FromBody] UpdateQuestionCategoryViewModel model)
    {
        var command = model.MapToCommand<UpdateQuestionCategoryCommand>();

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    #endregion
}