using System;

namespace HaoKao.GroupBookingService.Domain.Commands.GroupData;

/// <summary>
/// 删除命令
/// </summary>
/// <param name="Id">主键</param>
public record DeleteGroupDataCommand(
    Guid Id
) : Command("删除拼团资料");