using HaoKao.TenantService.Domain.Entities;
using HaoKao.TenantService.Domain.Enums;
using System.Collections.Generic;

namespace HaoKao.TenantService.Domain.Commands;

public record CreateTenantCommand(
    Guid? OtherId,
    string OtherName,
    string TenantName,
    string TenantNo,
    string AdminUserAcount,
    string AdminUserName,
    string AdminPhone,
    string AdminPassWord,
    SystemModule SystemModule,
    DateTime AnnualExamTime
) : Command("创建租户")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {
        validator.RuleFor(x => TenantName).MaximumLength(500).WithMessage("租户名称长度不能大于500");
        validator.RuleFor(x => TenantNo).MaximumLength(20).WithMessage("租户代码长度不能大于20");
        validator.RuleFor(x => AdminUserAcount).MaximumLength(36).WithMessage("租户管理员姓名长度不能大于36");
        validator.RuleFor(x => AdminPhone).MaximumLength(36).WithMessage("租户管理员手机号长度不能大于36");
    }
}

public record UpdateTenantCommand(
    Guid Id,
    Guid? OtherId,
    string OtherName,
    string TenantName,
    string TenantNo,
    string AdminUserName,
    string AdminPhone,
    string AdminPassWord,
    SystemModule SystemModule,
    DateTime AnnualExamTime
) : Command("修改租户")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {
        validator.RuleFor(x => TenantName).MaximumLength(500).WithMessage("租户名称长度不能大于500");
        validator.RuleFor(x => TenantNo).MaximumLength(20).WithMessage("租户代码长度不能大于20");
        validator.RuleFor(x => AdminPhone).MaximumLength(36).WithMessage("租户管理员手机号长度不能大于36");
    }
}

public record EnableDisabledExamCommand(Guid Id, EnableState StartState) : Command("启用/禁用");

public record SetPaymentConfigTenantCommand(Guid Id, List<PaymentConfig> PaymentConfigs) : Command("设置租户收款配置");