using HaoKao.BasicService.Domain.Commands.User;

namespace HaoKao.BasicService.Domain.Validations.User;

public class CreateUserCommandValidation : UserCommandValidation<CreateUserCommand>
{
    public CreateUserCommandValidation()
    {
        ValidationName();
        ValidationContactNumber();
        ValidationUserAccount();
        ValidationUserPassword();
    }

    public override bool IsErrorMessageDelay { get; set; } = true;
}