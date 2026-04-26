using Girvs.AuthorizePermission;
using HaoKao.BurialPointService.Domain.Entities;
using HaoKao.Common;
using Microsoft.AspNetCore.Authorization;

namespace HaoKao.BurialPointService.Application.Services.Management;


/// <summary>
/// 数据埋点分析服务-管理端
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.Management)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsJwt)]
public class BurialPointService(
    IStaticCacheManager cacheManager,
    IBurialPointRepository repository
) : IBurialPointService
{
    #region 初始参数

    private readonly IStaticCacheManager _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
    private readonly IBurialPointRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));

    #endregion

    #region 服务方法

    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    [HttpGet("{id:guid}")]
    public async Task<BrowseBurialPointViewModel> Get(Guid id)
    {
        var burialPoint = await _cacheManager.GetAsync(
              GirvsEntityCacheDefaults<BurialPoint>.ByIdCacheKey.Create(id.ToString()),
              async () => await _repository.GetByIdAsync(id)
            );

        if (burialPoint == null)
            throw new GirvsException("对应的埋点不存在", StatusCodes.Status404NotFound);

        return burialPoint.MapToDto<BrowseBurialPointViewModel>();
    }

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    [HttpGet]
    public async Task<BurialPointQueryViewModel> Get([FromQuery] BurialPointQueryViewModel queryViewModel)
    {
        var query = queryViewModel.MapToQuery<BurialPointQuery>();
        var tempQuery = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<BurialPoint>.QueryCacheKey.Create(query.GetCacheKey()), async () =>
            {
                await _repository.GetByQueryAsync(query);
                return query;
            });

        if (!query.Equals(tempQuery))
        {
            query.RecordCount = tempQuery.RecordCount;
            query.Result = tempQuery.Result;
        }

        return query.MapToQueryDto<BurialPointQueryViewModel, BurialPoint>();
    }
    #endregion
}