namespace HaoKao.DrawPrizeService.Domain.Commands.DrawPrize;
/// <summary>
/// 修改抽奖禁用启用
/// </summary>
/// <param name="Ids"></param>
/// <param name="Enable">禁用启用</param>
public record SetDrawPrizeEnableCommand(ICollection<Guid> Ids, bool Enable) : Command("修改抽奖禁用启用");