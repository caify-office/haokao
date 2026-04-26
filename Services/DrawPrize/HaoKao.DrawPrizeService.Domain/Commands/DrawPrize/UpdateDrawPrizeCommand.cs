namespace HaoKao.DrawPrizeService.Domain.Commands.DrawPrize;
/// <summary>
/// 更新抽奖命令
/// </summary>
/// <param name="Id">主键</param>
/// <param name="Title">名称</param>
/// <param name="BackgroundImageUrl">活动背景图</param>
/// <param name="StartTime">开始时间</param>
/// <param name="EndTime">结束时间</param>
/// <param name="Desc">说明</param>
public record UpdateDrawPrizeCommand(
   Guid Id,
   string Title,
   string BackgroundImageUrl,
   DateTime StartTime,
   DateTime EndTime,
   string Desc

) : Command("更新抽奖");