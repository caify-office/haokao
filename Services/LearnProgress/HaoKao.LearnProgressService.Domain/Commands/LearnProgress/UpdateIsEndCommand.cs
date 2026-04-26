using System;

namespace HaoKao.LearnProgressService.Domain.Commands.LearnProgress;

/// <summary>
/// 更新是否已学完
/// </summary>
/// <param name="Id">主键</param>
public record UpdateIsEndCommand(
    Guid Id
) : Command("更新是否已学完");