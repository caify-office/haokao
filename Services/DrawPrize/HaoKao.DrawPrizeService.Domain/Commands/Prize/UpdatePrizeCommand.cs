using HaoKao.DrawPrizeService.Domain.Entities;

namespace HaoKao.DrawPrizeService.Domain.Commands.Prize;
/// <summary>
/// 更新奖品命令
/// </summary>
/// <param name="Id">主键</param>
/// <param name="DrawPrizeId">所属抽奖活动Id</param>
/// <param name="Name">名称</param>
/// <param name="Count">奖品数量</param>
/// <param name="DisplayCount">显示数量</param>
/// <param name="WinningRange">中奖范围</param>
/// <param name="DesignatedStudents"></param>
public record UpdatePrizeCommand(
   Guid Id,
   Guid DrawPrizeId,
   string Name,
   Int32 Count,
   Int32 DisplayCount,
   WinningRange WinningRange,
   List<DesignatedStudent> DesignatedStudents

) : Command("更新奖品");