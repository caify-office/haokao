using HaoKao.OrderService.Application.ViewModels.PlatformPayer;

namespace HaoKao.OrderService.Application.Services.WeChat;

public interface IPlatformPayerWeChatService : IAppWebApiService, IManager
{
    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    Task<PlatformPayerQueryViewModel> Get(PlatformPayerQueryViewModel queryViewModel);
}