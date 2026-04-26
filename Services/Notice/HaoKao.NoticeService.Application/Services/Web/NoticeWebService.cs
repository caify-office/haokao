using HaoKao.NoticeService.Application.ViewModels;
using HaoKao.NoticeService.Domain.Models;
using HaoKao.NoticeService.Domain.Queries;
using HaoKao.NoticeService.Domain.Repositories;

namespace HaoKao.NoticeService.Application.Services.Web;

/// <summary>
/// 公告接口服务 - 网页端
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.WebSite)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class NoticeWebService(
    INoticeRepository repository,
    IStaticCacheManager cacheManager,
    IMapper mapper) : INoticeWebService
{
    private readonly INoticeRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly IStaticCacheManager _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

    /// <summary>
    /// 获取公告列表
    /// </summary>
    /// <param name="popup">是否弹出</param>
    /// <returns></returns>
    [HttpGet, AllowAnonymous]
    public async Task<List<BrowseNoticeViewModel>> Get(bool? popup)
    {
        if (!EngineContext.Current.HttpContext.Request.Headers.ContainsKey("TenantId"))
        {
            EngineContext.Current.HttpContext.Request.Headers["TenantId"] = Guid.Empty.ToString();
        }
        var query = new NoticeQuery { Popup = popup, Published = true };
        var key = GirvsEntityCacheDefaults<Notice>.QueryCacheKey.Create(query.GetCacheKey());
        var list = await _cacheManager.GetAsync(key, () => _repository.GetPublishedNoticeList(query));
        return _mapper.Map<List<BrowseNoticeViewModel>>(list);
    }
}