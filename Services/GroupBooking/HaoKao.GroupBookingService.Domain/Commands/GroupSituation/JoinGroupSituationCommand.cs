using System;

namespace HaoKao.GroupBookingService.Domain.Commands.GroupSituation;

/// <summary>
/// 参与拼团命令
/// </summary>
/// <param name="GroupDataId"></param>
/// <param name="GroupSituationId"></param>
/// <param name="Name"></param>
/// <param name="ImageUrl"></param>
public record JoinGroupSituationCommand(
    Guid GroupDataId,
    Guid GroupSituationId,
    string Name,
    string ImageUrl
) : Command("参与拼团");
