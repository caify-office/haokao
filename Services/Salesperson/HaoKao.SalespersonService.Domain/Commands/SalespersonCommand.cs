namespace HaoKao.SalespersonService.Domain.Commands;

/// <summary>
/// 创建销售人员命令
/// </summary>
/// <param name="RealName">真实姓名</param>
/// <param name="Phone">手机号</param>
/// <param name="EnterpriseWeChatUserId">企业微信Id</param>
/// <param name="EnterpriseWeChatUserName">企业微信昵称</param>
/// <param name="EnterpriseWeChatConfigId">企业微信配置Id</param>
public record CreateSalespersonCommand(
    string RealName,
    string Phone,
    string EnterpriseWeChatUserId,
    string EnterpriseWeChatUserName,
    Guid EnterpriseWeChatConfigId
) : Command("创建销售人员命令")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {
        validator.RuleFor(x => RealName).NotEmpty().WithMessage("真实姓名不能为空");
        validator.RuleFor(x => Phone).NotEmpty().WithMessage("手机号不能为空");

        // if (EnterpriseWeChatConfigId != Guid.Empty)
        // {
        //     validator.RuleFor(x => EnterpriseWeChatUserId).NotEmpty().WithMessage("企业微信Id不能为空");
        //     validator.RuleFor(x => EnterpriseWeChatUserName).NotEmpty().WithMessage("企业微信昵称不能为空");
        // }
    }
}

/// <summary>
/// 更新销售人员命令
/// </summary>
/// <param name="Id"></param>
/// <param name="RealName">真实姓名</param>
/// <param name="Phone">手机号</param>
/// <param name="EnterpriseWeChatUserId">企业微信用户Id</param>
/// <param name="EnterpriseWeChatUserName">企业微信昵称</param>
/// <param name="EnterpriseWeChatConfigId">企业微信配置Id</param>
public record UpdateSalespersonCommand(
    Guid Id,
    string RealName,
    string Phone,
    string EnterpriseWeChatUserId,
    string EnterpriseWeChatUserName,
    Guid EnterpriseWeChatConfigId
) : Command("更新销售人员命令")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {
        validator.RuleFor(x => RealName).NotEmpty().WithMessage("真实姓名不能为空");
        validator.RuleFor(x => Phone).NotEmpty().WithMessage("手机号不能为空");

        // if (EnterpriseWeChatConfigId != Guid.Empty)
        // {
        //     validator.RuleFor(x => EnterpriseWeChatUserId).NotEmpty().WithMessage("企业微信Id不能为空");
        //     validator.RuleFor(x => EnterpriseWeChatUserName).NotEmpty().WithMessage("企业微信昵称不能为空");
        // }
    }
}

/// <summary>
/// 删除销售人员命令
/// </summary>
/// <param name="Ids"></param>
public record DeleteSalespersonCommand(IReadOnlyList<Guid> Ids) : Command("删除销售人员命令");