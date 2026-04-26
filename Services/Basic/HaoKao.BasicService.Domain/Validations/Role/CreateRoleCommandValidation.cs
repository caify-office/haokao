using HaoKao.BasicService.Domain.Commands.Role;

namespace HaoKao.BasicService.Domain.Validations.Role;

public sealed class CreateRoleCommandValidation : RoleCommandValidation<CreateRoleCommand>
{
    public CreateRoleCommandValidation()
    {
        ValidationName();
    }
}