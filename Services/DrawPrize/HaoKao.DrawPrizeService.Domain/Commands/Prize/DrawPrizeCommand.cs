namespace HaoKao.DrawPrizeService.Domain.Commands.Prize;
/// <summary>
/// 抽取奖品命令
/// </summary>
/// <param name="DrawPrizeId">所属抽奖活动Id</param>
/// <param name="Phone"></param>
/// <param name="IsPaidStudents"></param>

public record DrawPrizeCommand(
    Guid DrawPrizeId,
    string Phone,
    bool IsPaidStudents
) : Command<Entities.Prize>("抽取奖品");