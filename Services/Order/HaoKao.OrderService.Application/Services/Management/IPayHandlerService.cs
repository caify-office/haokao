using HaoKao.OrderService.Application.PayHandler;

namespace HaoKao.OrderService.Application.Services.Management;

public interface IPayHandlerService : IAppWebApiService, IManager
{
    Task<List<IPayHandler>> Get();

    Task<IPayHandler> GetById(Guid id);

    dynamic GetPayerNeedParameterById(Guid id);

    Task<IPayHandler> GetByPlatformPayerId(Guid platformPayerId);
}