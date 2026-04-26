using Girvs.AuthorizePermission.Enumerations;
using Girvs.Configuration;
using Girvs.Driven.Extensions;
using Girvs.Extensions;
using Girvs.Infrastructure;
using HaoKao.Common;
using HaoKao.CouponService.Application.Configuration;
using HaoKao.CouponService.Application.Services.Refit;
using HaoKao.CouponService.Application.ViewModels.Coupon;
using HaoKao.CouponService.Application.ViewModels.Refit;
using HaoKao.CouponService.Domain.Commands.Coupon;
using HaoKao.CouponService.Domain.Models;
using HaoKao.CouponService.Domain.Queries;
using HaoKao.CouponService.Domain.Repositories;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

namespace HaoKao.CouponService.Application.Services.Management;

/// <summary>
/// 优惠券接口服务
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.Management)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsJwt)]
[ServicePermissionDescriptor(
    "优惠券",
    "97006461-302a-4d50-b200-b2116cb41f4a",
    "512",
    SystemModule.ExtendModule2 | SystemModule.ExtendModule3,
    3
)]
public class CouponService(
    IStaticCacheManager cacheManager,
    IMediatorHandler bus,
    INotificationHandler<DomainNotification> notifications,
    ICouponRepository repository,
    IWeiXinRemoteService remoteService
) : ICouponService
{
    #region 初始参数

    private readonly IStaticCacheManager _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly DomainNotificationHandler _notifications = (DomainNotificationHandler)notifications;
    private readonly ICouponRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly IWeiXinRemoteService _remoteService = remoteService ?? throw new ArgumentNullException(nameof(remoteService));

    #endregion

    #region 服务方法

    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="Id">主键</param>
    [HttpGet("{id:guid}")]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<BrowseCouponViewModel> Get(Guid Id)
    {
        var coupon = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<Coupon>.ByIdCacheKey.Create(Id.ToString()),
            () => _repository.GetByIdAsync(Id)
        ) ?? throw new GirvsException("对应的不存在", StatusCodes.Status404NotFound);

        return coupon.MapToDto<BrowseCouponViewModel>();
    }

    /// <summary>
    /// 根据主键数组获取指定
    /// </summary>
    /// <param name="ids">主键</param>
    [HttpPost]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<List<BrowseCouponViewModel>> GetByIds(Guid[] ids)
    {
        var couponList = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<Coupon>.ByIdCacheKey.Create(string.Join("_", ids.OrderBy(x => x)).ToMd5()),
            () => _repository.GetWhereAsync(x => ids.Contains(x.Id))
        );

        if (couponList is { Count: 0 }) throw new GirvsException("对应的不存在", StatusCodes.Status404NotFound);

        //调整排序
        var sorted = from id in ids
                     select couponList.FirstOrDefault(c => c.Id == id) into coupon
                     where coupon is not null
                     select coupon.MapToDto<BrowseCouponViewModel>();

        return sorted.ToList();
    }

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    [HttpGet]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<CouponQueryViewModel> Get([FromQuery] CouponQueryViewModel queryViewModel)
    {
        var query = queryViewModel.MapToQuery<CouponQuery>();
        var cacheKey = GirvsEntityCacheDefaults<Coupon>.QueryCacheKey.Create(query.GetCacheKey());
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

        var result = query.MapToQueryDto<CouponQueryViewModel, Coupon>();
        if (result.Result is { Count: > 0 })
        {
            foreach (var x in result.Result)
            {
                x.ReceiveCount = x.UserCoupons.Count;
                x.UseCount = x.UserCoupons.Count(p => p.IsUse);
            }
        }
        return result;
    }

    /// <summary>
    /// 创建
    /// </summary>
    /// <param name="model">新增模型</param>
    [HttpPost]
    [ServiceMethodPermissionDescriptor("创建", Permission.Post, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task Create([FromBody] CreateCouponViewModel model)
    {
        var command = model.MapToCommand<CreateCouponCommand>();
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
        var command = new DeleteCouponCommand(id);

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
    [ServiceMethodPermissionDescriptor("编辑", Permission.Edit, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task Update(Guid id, [FromBody] UpdateCouponViewModel model)
    {
        model.Id = id;
        var command = model.MapToCommand<UpdateCouponCommand>();

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 生成二维码
    /// </summary>
    /// <param name="couponId">优惠券code</param>
    /// <returns></returns>
    [HttpGet]
    [ServiceMethodPermissionDescriptor("合并生成二维码", Permission.Relation, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<string> ExportInstitutions(string couponId)
    {
        var tenantId = EngineContext.Current.ClaimManager.IdentityClaim.GetTenantId<Guid>();
        var commonConfig = Singleton<AppSettings>.Instance.ModuleConfigurations[nameof(CommonParamterConfig)] as CommonParamterConfig;
        var accessToken = await GetWeiXinAccessToken(commonConfig.AppKey, commonConfig.APPSecret);

        // 小程序页面路径
        var path = $"pages/callBack/callBack?action=coupon&couponId={couponId.Replace("-", "")}&tenantId={tenantId}";

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri($"{commonConfig.GetCodeUrl}?access_token={accessToken.Access_token}"),
            Headers =
            {
                { "responseType", "arraybuffer" },
            },
            Content = new StringContent("{\"path\":\"" + path + "\"}\r\n")
            {
                Headers =
                {
                    ContentType = new MediaTypeHeaderValue("application/json")
                }
            }
        };

        var client = new HttpClient();
        using var response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();
        var utf8Bytes = await response.Content.ReadAsByteArrayAsync();
        var base64String = Convert.ToBase64String(utf8Bytes);
        return base64String;
    }

    /// <summary>
    /// 读取小程序token
    /// </summary>
    /// <param name="appId"></param>
    /// <param name="secret"></param>
    /// <returns></returns>
    private Task<WeiXinAccessTokenViewModel> GetWeiXinAccessToken(string appId, string secret)
    {
        var cacheKey = GirvsEntityCacheDefaults<Coupon>.ByIdCacheKey.Create($"{nameof(GetWeiXinAccessToken)}:{appId}", cacheTime: 119);
        return _cacheManager.GetAsync(cacheKey, async () =>
        {
            var json = await _remoteService.GetAccessTokenAsync(new WeiXinAccessTokenBody
            {
                appid = appId,
                secret = secret,
                force_refresh = false
            });
            return JsonConvert.DeserializeObject<WeiXinAccessTokenViewModel>(json);
        });
    }

    [HttpGet]
    [ServiceMethodPermissionDescriptor("导出", Permission.Copy, UserType.TenantAdminUser | UserType.GeneralUser)]
    public void Export() { }
}

#endregion