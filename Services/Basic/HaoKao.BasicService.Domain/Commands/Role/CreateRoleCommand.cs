namespace HaoKao.BasicService.Domain.Commands.Role;

/// <summary>
/// 创建角色
/// </summary>
/// <param name="Name">角色名称</param>
/// <param name="Desc">描述</param>
/// <param name="UserIds">用户Id集合</param>
public record CreateRoleCommand(string Name, string Desc, Guid[] UserIds) : RoleCommand(Name, Desc, UserIds, "创建角色")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {
        validator.RuleFor(role => Desc)
                 .MaximumLength(500).WithMessage("角色描述长度不能大于500");
    }
}