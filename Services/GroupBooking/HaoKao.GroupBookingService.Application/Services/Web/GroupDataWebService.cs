using HaoKao.Common;
using HaoKao.GroupBookingService.Application.Services.Management;
using HaoKao.GroupBookingService.Application.ViewModels.GroupData;

namespace HaoKao.GroupBookingService.Application.Services.Web;

/// <summary>
/// 拼团资料接口服务-web端
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.WebSite)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class GroupDataWebService(IGroupDataService groupDataService) : IGroupDataWebService
{
    #region 初始参数

    private readonly IGroupDataService _groupDataService = groupDataService ?? throw new ArgumentNullException(nameof(groupDataService));

    #endregion

    #region 服务方法

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    [HttpGet]
    [AllowAnonymous]
    public Task<GroupDataQueryViewModel> Get([FromQuery] GroupDataQueryViewModel queryViewModel)
    {
        return _groupDataService.Get(queryViewModel);
    }

    #endregion
}