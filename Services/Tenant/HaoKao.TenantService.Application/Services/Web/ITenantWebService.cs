using HaoKao.TenantService.Application.ViewModels;
using System.Collections.Generic;

namespace HaoKao.TenantService.Application.Services.Web;

public interface ITenantWebService : IAppWebApiService
{
    Task<List<TenantQueryListViewModel>> Get();

    Task<TenantQueryListViewModel> GetCurrent();

    Task<TenantQueryListViewModel> GetAsync(Guid id);
}