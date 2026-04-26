using HaoKao.BasicService.Domain.Commands.Role;

namespace HaoKao.BasicService.Domain.Validations.Role;

public sealed class UpdateRoleCommandValidation : RoleCommandValidation<UpdateRoleCommand>
{
    public UpdateRoleCommandValidation()
    {
        ValidationName();
    }
}