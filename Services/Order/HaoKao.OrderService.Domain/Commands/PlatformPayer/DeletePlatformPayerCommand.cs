namespace HaoKao.OrderService.Domain.Commands.PlatformPayer;

/// <summary>
/// 删除平台配置的支付列表命令
/// </summary>
/// <param name="Id">主键</param>
public record DeletePlatformPayerCommand(Guid Id) : Command("删除平台配置的支付列表");