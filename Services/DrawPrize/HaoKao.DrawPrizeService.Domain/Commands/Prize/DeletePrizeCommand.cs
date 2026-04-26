namespace HaoKao.DrawPrizeService.Domain.Commands.Prize;
/// <summary>
/// 删除奖品命令
/// </summary>
/// <param name="Id">主键</param>
public record DeletePrizeCommand(
    Guid Id
) : Command("删除奖品");