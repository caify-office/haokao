using HaoKao.BasicService.Domain.Commands.User;

namespace HaoKao.BasicService.Domain.Validations.User;

public class UpdateUserCommandValidation : UserCommandValidation<UpdateUserCommand>
{
    public UpdateUserCommandValidation()
    {
        ValidationName();
        ValidationContactNumber();
        // ValidationUserAccount();
        ValidationUserPassword();
        ValidationContactNumber();
    }

    public override bool IsErrorMessageDelay { get; set; } = true;
}