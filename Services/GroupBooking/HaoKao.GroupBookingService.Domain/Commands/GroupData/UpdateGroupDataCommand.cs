using FluentValidation;
using System;
using System.Collections.Generic;

namespace HaoKao.GroupBookingService.Domain.Commands.GroupData;

/// <summary>
/// 更新命令
/// </summary>
/// <param name="Id"></param>
/// <param name="DataName"></param>
/// <param name="SuitableSubjects"></param>
/// <param name="PeopleNumber"></param>
/// <param name="BasePeopleNumber"></param>
/// <param name="LimitTime"></param>
/// <param name="Document"></param>
/// <param name="State"></param>
public record UpdateGroupDataCommand(
    Guid Id,
    string DataName,
    List<Guid> SuitableSubjects,
    int PeopleNumber,
    int BasePeopleNumber,
    int LimitTime,
    string Document,
    bool State
) : Command("更新拼团资料")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {
        validator.RuleFor(x => DataName)
                 .NotEmpty().WithMessage("不能为空")
                 .MaximumLength(50).WithMessage("长度不能大于50")
                 .MinimumLength(2).WithMessage("长度不能小于2");
    }
}