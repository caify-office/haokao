using HaoKao.TenantService.Application.ViewModels;
using System.Collections.Generic;

namespace HaoKao.TenantService.Application.Services.App;

public interface ITenantAppService : IAppWebApiService, IManager
{
    Task<List<TenantQueryListViewModel>> Get();

    Task<TenantQueryListViewModel> GetCurrent();

    Task<TenantQueryListViewModel> GetAsync(Guid id);
}