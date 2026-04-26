using System;
using System.Collections.Generic;

namespace HaoKao.GroupBookingService.Domain.Commands.GroupSituation;

/// <summary>
/// 发起拼团命令
/// </summary>
/// <param name="GroupDataId"></param>
/// <param name="DataName"></param>
/// <param name="SuitableSubjects"></param>
/// <param name="PeopleNumber"></param>
/// <param name="LimitTime"></param>
/// <param name="Document"></param>
/// <param name="Name"></param>
/// <param name="ImageUrl"></param>
public record CreateGroupSituationCommand(
    Guid GroupDataId,
    string DataName,
    List<Guid> SuitableSubjects,
    int PeopleNumber,
    int LimitTime,
    string Document,
    string Name,
    string ImageUrl
) : Command<Guid>("创建拼团");