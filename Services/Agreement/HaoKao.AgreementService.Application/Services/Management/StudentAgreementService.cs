using HaoKao.AgreementService.Application.ViewModels.StudentAgreement;
using HaoKao.AgreementService.Domain.Commands;
using HaoKao.AgreementService.Domain.Entities;
using HaoKao.AgreementService.Domain.Queries;
using HaoKao.AgreementService.Domain.Repositories;

namespace HaoKao.AgreementService.Application.Services.Management;

/// <summary>
/// 学员协议接口服务
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.Management)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsJwt)]
[ServicePermissionDescriptor(
    "学员协议",
    "c372b4a3-41fb-41ee-a649-0d5c6d87e69b",
    "128",
    SystemModule.ExtendModule2 | SystemModule.ExtendModule3,
    3
)]
public class StudentAgreementService(
    IStaticCacheManager cacheManager,
    IMediatorHandler bus,
    INotificationHandler<DomainNotification> notifications,
    IStudentAgreementRepository repository,
    IMapper mapper
) : IStudentAgreementService
{
    #region 初始参数

    private readonly IStaticCacheManager _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly DomainNotificationHandler _notifications = (DomainNotificationHandler)notifications;
    private readonly IStudentAgreementRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

    private const UserType _BaseUserType = UserType.TenantAdminUser | UserType.GeneralUser;

    private const UserType _UserTypeOfView = _BaseUserType;
    private const UserType _UserTypeOfEdit = _BaseUserType;

    #endregion

    #region 服务方法

    /// <summary>
    /// 根据主键获取指定学员协议
    /// </summary>
    /// <param name="id">主键</param>
    [HttpGet("{id:guid}")]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, _UserTypeOfView)]
    public async Task<BrowseStudentAgreementViewModel> Get(Guid id)
    {
        var entity = await GetEntity(id);
        return entity.MapToDto<BrowseStudentAgreementViewModel>();
    }

    /// <summary>
    /// 根据主键获取指定学员协议
    /// </summary>
    /// <param name="id">主键</param>
    [NonAction]
    public Task<StudentAgreement> GetEntity(Guid id)
    {
        return _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<StudentAgreement>.ByIdCacheKey.Create(id.ToString()),
            () => _repository.GetByIdAsync(id)
        ) ?? throw new GirvsException("对应的学员协议不存在", StatusCodes.Status404NotFound);
    }

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    [HttpGet]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, _UserTypeOfView)]
    public async Task<QueryStudentAgreementViewModel> Get([FromQuery] QueryStudentAgreementViewModel queryViewModel)
    {
        var query = queryViewModel.MapToQuery<StudentAgreementQuery>();
        query.OrderBy = nameof(StudentAgreement.CreateTime);
        var cacheKey = GirvsEntityCacheDefaults<StudentAgreement>.QueryCacheKey.Create(query.GetCacheKey());
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

        return query.MapToQueryDto<QueryStudentAgreementViewModel, StudentAgreement>();
    }

    /// <summary>
    /// 创建学员协议
    /// </summary>
    /// <param name="model">新增模型</param>
    [NonAction]
    public async Task Create(CreateStudentAgreementViewModel model)
    {
        var command = _mapper.Map<CreateStudentAgreementCommand>(model);

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 根据主键更新指定学员协议
    /// </summary>
    /// <param name="id">主键</param>
    /// <param name="model">新增模型</param>
    [HttpPut("{id:guid}")]
    [ServiceMethodPermissionDescriptor("修改", Permission.Edit, _UserTypeOfEdit)]
    public async Task Update(Guid id, [FromBody] UpdateStudentAgreementViewModel model)
    {
        model.Id = id;
        var command = _mapper.Map<UpdateStudentAgreementCommand>(model);

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    #endregion
}