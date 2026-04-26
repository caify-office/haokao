using HaoKao.SalespersonService.Application.ViewModels;

namespace HaoKao.SalespersonService.Application.Interfaces;

public interface IEnterpriseWeChatConfigService : IAppWebApiService, IManager
{
    /// <summary>
    /// 根据租户获取
    /// </summary>
    /// <returns></returns>
    Task<BrowseEnterpriseWeChatConfigViewModel> Get();

    /// <summary>
    /// 保存企业微信配置
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task Create(CreateEnterpriseWeChatConfigViewModel model);
}