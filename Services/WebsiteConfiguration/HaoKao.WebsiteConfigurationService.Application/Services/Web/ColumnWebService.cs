using Girvs.AuthorizePermission;
using HaoKao.Common;
using HaoKao.WebsiteConfigurationService.Application.Services.Management;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;

namespace HaoKao.WebsiteConfigurationService.Application.Services.Web;

/// <summary>
///  栏目接口服务-Web端接口
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.WebSite)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
[AllowAnonymous]
public class ColumnWebService(
    IStaticCacheManager cacheManager,
    IColumnRepository repository,
    IColumnService columnService) : IColumnWebService
{
    #region 初始参数

    private readonly IColumnService _columnService = columnService ?? throw new ArgumentNullException(nameof(columnService));
    private readonly IStaticCacheManager _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
    private readonly IColumnRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));

    #endregion

    #region 服务方法
    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    [HttpGet("{id}")]
    public Task<BrowseColumnViewModel> Get(Guid id)
    {
        return _columnService.Get(id);
    }


    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    [HttpGet]
    public Task<ColumnQueryViewModel> Get([FromQuery] ColumnQueryViewModel queryViewModel)
    {
        return _columnService.Get(queryViewModel);
    }

    /// <summary>
    /// 根据域名和英文名查询符合条件栏目下面的子栏目信息
    /// </summary>
    /// <param name="model"></param>
    [HttpPost]
    public async Task<List<SimpleColumnQueryListViewModel>> GetChildrenColumn([FromBody] GetChildrenColumnViewModel model)
    {
        return await _cacheManager.GetAsync(GirvsEntityCacheDefaults<Column>.QueryCacheKey.Create($"ChildrenColumn:{model.DomainName}_{model.EnglishName}"), async () =>
        {
            var result = await _repository.GetChildrenColumnByDomainNameAndEnglishName(model.DomainName, model.EnglishName);
            var list = new List<SimpleColumnQueryListViewModel>();
            result.ForEach(x =>
            {
                list.Add(new SimpleColumnQueryListViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    EnglishName = x.EnglishName,
                    Icon = x.Icon,
                    ActiveIcon = x.ActiveIcon
                });
            });
            return list;
        });
    }


    #endregion
}