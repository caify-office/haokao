namespace HaoKao.DrawPrizeService.Domain.Commands.DrawPrizeRecord;
/// <summary>
/// 创建抽奖记录命令
/// </summary>
/// <param name="DrawPrizeId">所属抽奖活动Id</param>
/// <param name="Phone">手机号</param>
/// <param name="PrizeName">奖品名称</param>
public record CreateDrawPrizeRecordCommand(
    Guid DrawPrizeId,
    string Phone,
    string PrizeName
) : Command("创建抽奖记录");