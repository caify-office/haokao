using HaoKao.Common;
using HaoKao.OpenPlatformService.Application.ViewModels.ClientViewModel;
using HaoKao.OpenPlatformService.Domain.Entities;
using HaoKao.OpenPlatformService.Domain.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace HaoKao.OpenPlatformService.Application.Services.Web;

/// <summary>
/// 客户端接口服务
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.WebSite)]
public class ClientWebService(
    IStaticCacheManager cacheManager,
    IAccessClientRepository repository
) : IClientWebService
{
    private readonly IStaticCacheManager _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
    private readonly IAccessClientRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));

    /// <summary>
    /// 根据clientId获取详情
    /// </summary>
    /// <param name="clientId"></param>
    /// <returns></returns>
    [HttpGet("{clientId:guid}"), AllowAnonymous]
    public async Task<BrowseClientViewModel> Get(string clientId)
    {
        var accessClient = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<AccessClient>.ByIdCacheKey.Create(clientId.ToString()),
            () => _repository.GetAsync(x => x.ClientId == clientId)
        ) ?? throw new GirvsException("对应的不存在", StatusCodes.Status404NotFound);

        return accessClient.ConvertToViewModel();
    }
}