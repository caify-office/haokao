namespace HaoKao.OrderService.Domain.Commands.PlatformPayer;

/// <summary>
/// 设置启用禁用
/// </summary>
/// <param name="Id"></param>
/// <param name="UseState"></param>
public record SetPlatformPayerUseStateCommand(Guid Id, bool UseState) : Command("设置启用禁用");