using HaoKao.OrderService.Application.PayHandler;
using HaoKao.OrderService.Application.PayHandler.Config;
using HaoKao.OrderService.Domain.Repositories;

namespace HaoKao.OrderService.Application.Services.Management;

/// <summary>
/// 后台管理——平台支付者服务
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.Management)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsJwt)]
public class PayHandlerService(IPlatformPayerRepository platformPayerRepository) : IPayHandlerService
{
    /// <summary>
    /// 获取支付者列表
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public Task<List<IPayHandler>> Get()
    {
        var payers = EngineContext.Current.ResolveAll<IPayHandler>().ToList();
        return Task.FromResult(payers);
    }

    [NonAction]
    public Task<IPayHandler> GetById(Guid id)
    {
        var payers = EngineContext.Current.ResolveAll<IPayHandler>().ToList();
        var payer = payers.FirstOrDefault(x => x.PayHandlerId == id);
        return Task.FromResult(payer);
    }

    [NonAction]
    public async Task<IPayHandler> GetByPlatformPayerId(Guid platformPayerId)
    {
        var platformPayer = await platformPayerRepository.GetByIdAsync(platformPayerId);

        if (platformPayer == null || !platformPayer.VerificationDataSecurity()) throw new GirvsException("未找到对应的支付者，或支付者配置信息已被非法篡改");

        var payHandler = await GetById(platformPayer.PayerId);
        payHandler.SetConfig(platformPayer.Config);

        return payHandler;
    }

    /// <summary>
    /// 根据Id获取支付者详情列表
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id:guid}")]
    public dynamic GetPayerNeedParameterById(Guid id)
    {
        var payers = EngineContext.Current.ResolveAll<IPayHandler>().ToList();
        var payer = payers.FirstOrDefault(x => x.PayHandlerId == id);
        var ps = payer?.GetPayerNeedParameterType().GetProperties();

        return ps?.Select(x =>
        {
            var type = x.GetCustomAttribute<ConfigTypeAttribute>()?.ConfigType ?? ConfigType.String;
            var tip = x.GetCustomAttribute<ConfigTipAttribute>()?.Tip ?? string.Empty;
            return new
            {
                x.Name,
                Tip = tip,
                Type = type,
            };
        });
    }
}