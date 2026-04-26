using HaoKao.TenantService.Application.ViewModels;
using System.Collections.Generic;

namespace HaoKao.TenantService.Application.Services.WeChat;

public interface ITenantWeChatService : IAppWebApiService
{
    Task<List<TenantQueryListViewModel>> Get();

    Task<TenantQueryListViewModel> GetCurrent();

    Task<TenantQueryListViewModel> GetAsync(Guid id);
}