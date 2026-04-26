using Girvs.AuthorizePermission.Enumerations;
using HaoKao.DrawPrizeService.Domain.Entities;

namespace HaoKao.DrawPrizeService.Application.Services.Management;

/// <summary>
/// 抽奖记录服务-管理端
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.Management)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsJwt)]
[ServicePermissionDescriptor(
    "抽奖记录管理",
    "90ea29b4-50a2-c097-d78e-041ad1ef63b9",
    "512",
    SystemModule.ExtendModule2,
    1
)]
public class DrawPrizeRecordService(
    IStaticCacheManager cacheManager,
    IDrawPrizeRecordRepository repository
) : IDrawPrizeRecordService
{
    #region 初始参数

    private readonly IStaticCacheManager _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
    private readonly IDrawPrizeRecordRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));

    #endregion

    #region 服务方法

    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    [HttpGet("{id:guid}")]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<BrowseDrawPrizeRecordViewModel> Get(Guid id)
    {
        var drawPrizeRecord = await _cacheManager.GetAsync(
              GirvsEntityCacheDefaults<DrawPrizeRecord>.ByIdCacheKey.Create(id.ToString()),
              () => _repository.GetByIdAsync(id)
        ) ?? throw new GirvsException("对应的抽奖记录不存在", StatusCodes.Status404NotFound);

        return drawPrizeRecord.MapToDto<BrowseDrawPrizeRecordViewModel>();
    }

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    [HttpGet]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<DrawPrizeRecordQueryViewModel> Get([FromQuery] DrawPrizeRecordQueryViewModel queryViewModel)
    {
        var query = queryViewModel.MapToQuery<DrawPrizeRecordQuery>();
        query.OrderBy = "CreateTime";
        var cacheKey = GirvsEntityCacheDefaults<DrawPrizeRecord>.QueryCacheKey.Create(query.GetCacheKey());
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

        return query.MapToQueryDto<DrawPrizeRecordQueryViewModel, DrawPrizeRecord>();
    }

    #endregion
}