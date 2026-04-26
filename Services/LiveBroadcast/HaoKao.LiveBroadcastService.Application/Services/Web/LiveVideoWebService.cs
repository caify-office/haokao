using Girvs.AuthorizePermission;
using Girvs.Extensions;
using HaoKao.Common;
using HaoKao.LiveBroadcastService.Application.Services.Management;
using HaoKao.LiveBroadcastService.Application.ViewModels.LiveMessage;
using HaoKao.LiveBroadcastService.Domain.Entities;
using HaoKao.LiveBroadcastService.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;

namespace HaoKao.LiveBroadcastService.Application.Services.Web;

/// <summary>
/// 视频直播接口服务-Web端
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.WebSite)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
[AllowAnonymous]
public class LiveVideoWebService(
    IStaticCacheManager cacheManager,
    ILiveVideoRepository repository,
    ILiveVideoService liveVideoService,
    ILiveAdministratorWebService liveAdministratorWebService,
    OnlineUserState onlineUserState
) : ILiveVideoWebService
{
    #region 初始参数

    private readonly IStaticCacheManager _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
    private readonly ILiveVideoRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly ILiveVideoService _liveVideoService = liveVideoService ?? throw new ArgumentNullException(nameof(liveVideoService));
    private readonly ILiveAdministratorWebService _liveAdministratorWebService = liveAdministratorWebService ?? throw new ArgumentNullException(nameof(liveAdministratorWebService));
    private readonly OnlineUserState _onlineUserState = onlineUserState ?? throw new ArgumentNullException(nameof(onlineUserState));

    #endregion

    #region 服务方法

    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    [HttpGet("{id}")]
    public Task<BrowseLiveVideoViewModel> Get(Guid id)
    {
        return _liveVideoService.Get(id);
    }

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    [HttpGet]
    public async Task<LiveVideoQueryViewModel> Get([FromQuery] LiveVideoQueryViewModel queryViewModel)
    {
        var queryCacheKey = JsonConvert.SerializeObject(queryViewModel).ToMd5();
        queryCacheKey += "Web";
        var query = queryViewModel.MapToQuery<LiveVideoQuery>();
        var tempQuery = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<LiveVideo>.QueryCacheKey.Create(queryCacheKey), async () =>
            {
                if (query.LiveStatus == LiveStatus.Ended)
                {
                    //查询结束的直播，需要按照开始时间倒序排序
                    await _repository.GetByQueryAsync(query);
                }
                else
                {
                    //其他情况，按照开始时间正序排序
                    await _repository.GetByQueryOrderByAsync(query);
                }

                return query;
            });

        if (!query.Equals(tempQuery))
        {
            query.RecordCount = tempQuery.RecordCount;
            query.Result = tempQuery.Result;
        }
        var result = query.MapToQueryDto<LiveVideoQueryViewModel, LiveVideo>();
        //统计在线人数
        result.Result.ForEach(x =>
        {
            _onlineUserState.OnlineCount.TryGetValue(x.Id, out var viewCount);
            x.ViewNumber = viewCount;
        });

        return result;
    }


    /// <summary>
    /// 查询最新直播
    /// </summary>
    [HttpGet]
    public  Task<LiveVideoQueryViewModel> GetNewLive()
    {
        var queryViewModel = new LiveVideoQueryViewModel
        {
            PageIndex = 1,
            PageSize = 10,
            QueryStartTime = DateTime.Now.Date,
            QueryEndTime = DateTime.Now.Date.AddDays(7)
        };
        return Get(queryViewModel);
    }


    /// <summary>
    /// 根据主键修改直播状态(助教才有权限使用)
    /// </summary>
    /// <param name="model">新增模型</param>
    [HttpPut]
    public  async Task SetLiveVideoStatus([FromBody] SetLiveVideoStatusViewModel model)
    {
        //判定是否是助教
        var isAdmin = await _liveAdministratorWebService.IsLiveAdmin();
        if (!isAdmin)
        {
            throw new GirvsException("当前操作无权限,请联系管理员处理",StatusCodes.Status400BadRequest);
        }
        await  _liveVideoService.SetLiveVideoStatus(model);
    }

    #endregion
}