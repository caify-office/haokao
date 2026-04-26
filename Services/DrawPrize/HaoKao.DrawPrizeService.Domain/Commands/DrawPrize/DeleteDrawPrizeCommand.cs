namespace HaoKao.DrawPrizeService.Domain.Commands.DrawPrize;
/// <summary>
/// 删除抽奖命令
/// </summary>
/// <param name="Ids">主键</param>
public record DeleteDrawPrizeCommand(
    Guid[] Ids
) : Command("删除抽奖");