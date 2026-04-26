using HaoKao.TenantService.Application.ViewModels;

namespace HaoKao.TenantService.Application.Services.Management;

public interface ITenantService : IAppWebApiService, IManager
{
    Task<UpdateTenantViewModel> GetAsync(Guid id);

    Task<TenantQueryViewModel> GetAsync(TenantQueryViewModel queryModel);

    Task<CreateTenantViewModel> CreateAsync(CreateTenantViewModel model);

    Task<UpdateTenantViewModel> UpdateAsync(UpdateTenantViewModel model);

    Task<SetPaymentConfigViewModel> SetPaymentConfigAsync(SetPaymentConfigViewModel model);

    Task<bool> ExistByNo(string no);

    Task<EnableDisabledTenantViewModel> EnableDisabledAsync(EnableDisabledTenantViewModel model);
}