namespace HaoKao.BasicService.Domain.Commands.Role;

/// <summary>
/// 添加角色用户
/// </summary>
/// <param name="RoleId">角色Id集合</param>
/// <param name="UserIds">用户Id集合</param>
public record AddRoleUserCommand(Guid RoleId, IList<Guid> UserIds) : Command("添加角色用户")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {
        validator.RuleFor(x => RoleId).NotEqual(Guid.Empty).WithMessage("角色Id不能为空或默认值");
    }
}