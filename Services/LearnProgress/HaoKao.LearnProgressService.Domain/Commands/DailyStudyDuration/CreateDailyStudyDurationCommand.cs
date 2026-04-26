using FluentValidation;
using System;

namespace HaoKao.LearnProgressService.Domain.Commands.DailyStudyDuration;
/// <summary>
/// 创建命令
/// </summary>
/// <param name="ProductId">产品Id</param>
/// <param name="SubjectId">对应的科目Id</param>
/// <param name="StudyDuration">学习时长间隔</param>
public record CreateDailyStudyDurationCommand(
    Guid ProductId,
    Guid SubjectId,
    decimal StudyDuration
) : Command("创建")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {
    }
}