using System;

namespace HaoKao.LearnProgressService.Domain.Commands.LearnProgress;
/// <summary>
/// 删除命令
/// </summary>
/// <param name="Id">主键</param>
public record DeleteLearnProgressCommand(
    Guid Id
) : Command("删除");