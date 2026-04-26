using Girvs.AuthorizePermission;
using HaoKao.BurialPointService.Application.ViewModels.BrowseRecord;
using HaoKao.Common;
using Microsoft.AspNetCore.Authorization;
using HaoKao.BurialPointService.Application.Services.Management;

namespace HaoKao.BurialPointService.Application.Services.Web;

/// <summary>
/// 数据埋点服务-Web端
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.WebSite)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class BrowseRecordWebService(IBrowseRecordService browseRecordService) : IBrowseRecordWebService
{
    #region 初始参数

    private readonly IBrowseRecordService _browseRecordService = browseRecordService ?? throw new ArgumentNullException(nameof(browseRecordService));

    #endregion

    #region 服务方法

    /// <summary>
    /// 创建浏览记录
    /// </summary>
    /// <param name="model">新增模型</param>
    [HttpPost]
    public Task Create([FromBody] CreateBrowseRecordViewModel model)
    {
        model.BelongingPortType = Domain.Enums.BelongingPortType.WebSite;
        return _browseRecordService.Create(model);
    }

    #endregion
}