using FluentValidation;
using System;

namespace HaoKao.LearnProgressService.Domain.Commands.LearnProgress;
/// <summary>
/// 更新命令
/// </summary>
/// <param name="Id">主键</param>
/// <param name="Progress">学习时长(单位:s)</param>
/// <param name="MaxProgress">观看视频最大长度(单位:s)</param>
public record UpdateLearnProgressCommand(
   Guid Id,
   float Progress,
   float MaxProgress

) : Command("更新")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {

   

    }
}