namespace HaoKao.BasicService.Domain.Commands.Role;

/// <summary>
/// 更新角色
/// </summary>
/// <param name="Id">角色Id</param>
/// <param name="Name">角色名称</param>
/// <param name="Desc">描述</param>
/// <param name="UserIds">用户Id集合</param>
public record UpdateRoleCommand(Guid Id, string Name, string Desc, Guid[] UserIds) : RoleCommand(Name, Desc, UserIds, "更新角色")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {
        validator.RuleFor(x => Id).NotEqual(Guid.Empty).WithMessage("角色Id不能为空或者默认值");
        validator.RuleFor(role => Desc).MaximumLength(500).WithMessage("角色描述长度不能大于500");
    }
}