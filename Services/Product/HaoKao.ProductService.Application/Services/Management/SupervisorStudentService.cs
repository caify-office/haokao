using Girvs.Driven.Extensions;
using HaoKao.ProductService.Application.ViewModels.SupervisorStudent;
using HaoKao.ProductService.Domain.Commands.SupervisorStudent;
using HaoKao.ProductService.Domain.Entities;
using HaoKao.ProductService.Domain.Queries;
using HaoKao.ProductService.Domain.Repositories;

namespace HaoKao.ProductService.Application.Services.Management;

/// <summary>
/// 督学学员接口服务
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.Management)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsJwt)]

public class SupervisorStudentService : ISupervisorStudentService
{
    #region 初始参数

    private readonly IStaticCacheManager _cacheManager;
    private readonly IMediatorHandler _bus;
    private readonly DomainNotificationHandler _notifications;
    private readonly ISupervisorStudentRepository _repository;
    private readonly ISupervisorClassService _classService;
    private readonly IProductService _productService;

    #endregion

    #region 构造函数

    /// <summary>
    /// 构造函数
    /// </summary>
    public SupervisorStudentService(
        IStaticCacheManager cacheManager,
        IMediatorHandler bus,
        INotificationHandler<DomainNotification> notifications,
        ISupervisorStudentRepository repository,
        ISupervisorClassService classService,
        IProductService productService)
    {
        _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
        _bus = bus ?? throw new ArgumentNullException(nameof(bus));
        _notifications = (DomainNotificationHandler)notifications;
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _classService = classService ?? throw new ArgumentNullException(nameof(classService));
        _productService = productService ?? throw new ArgumentNullException(nameof(productService));
    }

    #endregion

    #region 服务方法


    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    [HttpGet]
    public async Task<SupervisorStudentQueryViewModel> Get([FromQuery]SupervisorStudentQueryViewModel queryViewModel)
    {
       
        var query = queryViewModel.MapToQuery<SupervisorStudentQuery>();
        var tempQuery = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<SupervisorStudent>.QueryCacheKey.Create(query.GetCacheKey()), async () =>
            {
                //先统计数据
                await UpdateStatisticsData(queryViewModel.SupervisorClassId);
                await _repository.GetByQueryAsync(query);
                return query;
            });

        if (!query.Equals(tempQuery))
        {
            query.RecordCount = tempQuery.RecordCount;
            query.Result = tempQuery.Result;
        }

        return query.MapToQueryDto<SupervisorStudentQueryViewModel, SupervisorStudent>();
    }
            
    /// <summary>
    /// 创建督学学员
    /// </summary>
    /// <param name="model">新增模型</param>
    [HttpPost]
    [ServiceMethodPermissionDescriptor("新增", Permission.Post, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task Create([FromBody]CreateSupervisorStudentViewModel model)
    {
        //学员加人班级条件：a.有产品权限的学员才能加人班级；b.同一个学员不能同时加入产品相同的两个班级）
        var supervisorClass = await _classService.Get(model.SupervisorClassId);
        var myProduct = await _productService.GetMyProduct(Domain.Enums.ProductType.Course, model.RegisterUserId);
        var userId = EngineContext.Current.ClaimManager.GetUserId().ToGuid();
        if (!myProduct.Any(x => x.Id == supervisorClass.ProductId))
        {
           
            throw new GirvsException("有产品权限的学员才能加入班级",StatusCodes.Status500InternalServerError);
        }

        if (_repository.ExistSupervisorClass(model.RegisterUserId, supervisorClass.ProductId,supervisorClass.Id))
        {
            if (userId == model.RegisterUserId) return;

            throw new GirvsException("学员已加入该班级，请勿重复添加", StatusCodes.Status400BadRequest);
        }
        if (_repository.ExistSupervisorClass(model.RegisterUserId, supervisorClass.ProductId))
        {
            if (userId == model.RegisterUserId) return;
            throw new GirvsException("同一个学员不能同时加入产品相同的两个班级", StatusCodes.Status400BadRequest);
        }

        var command = model.MapToCommand<CreateSupervisorStudentCommand>();
        
        await _bus.SendCommand(command);
        
        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 根据主键删除指定督学学员
    /// </summary>
    /// <param name="id">主键</param>
    [HttpDelete("{id}")]
    [ServiceMethodPermissionDescriptor("删除", Permission.Delete, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task Delete(Guid id)
    {
        var command = new DeleteSupervisorStudentCommand(id);

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 根据主键更新指定督学学员
    /// </summary>
    /// <param name="model">新增模型</param>
    [HttpPut]
    [ServiceMethodPermissionDescriptor("编辑", Permission.Edit, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task Update([FromBody] UpdateSupervisorStudentViewModel model)
    {
        var command = model.MapToCommand<UpdateSupervisorStudentCommand>();
        
        await _bus.SendCommand(command);
        
        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }
    /// <summary>
    /// 更新督学学员统计数据
    /// </summary>
    [HttpPut]
    [ServiceMethodPermissionDescriptor("编辑", Permission.Edit, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task UpdateStatisticsData(Guid supervisorClassId)
    {
        var supervisorClass = await _classService.Get(supervisorClassId);
        var command = new UpdateStudentStatisticsDataCommand(supervisorClassId,supervisorClass.ProductId);

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    #endregion
}