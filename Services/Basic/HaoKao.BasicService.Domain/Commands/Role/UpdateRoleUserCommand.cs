namespace HaoKao.BasicService.Domain.Commands.Role;

/// <summary>
/// 更新角色用户
/// </summary>
/// <param name="Id">角色用户Id</param>
/// <param name="UserIds">用户Id集合</param>
public record UpdateRoleUserCommand(Guid Id, Guid[] UserIds) : Command("更新角色用户")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {
        validator.RuleFor(x => Id).NotEqual(Guid.Empty).WithMessage("角色用户Id不能为空或默认值");
    }
}