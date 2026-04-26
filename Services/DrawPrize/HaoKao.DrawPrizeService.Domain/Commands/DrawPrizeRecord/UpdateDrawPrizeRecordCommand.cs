namespace HaoKao.DrawPrizeService.Domain.Commands.DrawPrizeRecord;
/// <summary>
/// 更新抽奖记录命令
/// </summary>
/// <param name="Id">主键</param>
/// <param name="DrawPrizeId">所属抽奖活动Id</param>
/// <param name="Phone">手机号</param>
/// <param name="PrizeName">奖品名称</param>
public record UpdateDrawPrizeRecordCommand(
   Guid Id,
   Guid DrawPrizeId,
   string Phone,
   string PrizeName

) : Command("更新抽奖记录");