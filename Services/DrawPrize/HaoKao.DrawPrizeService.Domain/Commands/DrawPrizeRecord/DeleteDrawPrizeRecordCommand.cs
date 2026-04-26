namespace HaoKao.DrawPrizeService.Domain.Commands.DrawPrizeRecord;
/// <summary>
/// 删除抽奖记录命令
/// </summary>
/// <param name="Id">主键</param>
public record DeleteDrawPrizeRecordCommand(
    Guid Id
) : Command("删除抽奖记录");