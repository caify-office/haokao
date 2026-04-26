namespace HaoKao.ProductService.Domain.Commands.SupervisorStudent;

/// <summary>
/// 创建督学学员命令
/// </summary>
/// <param name="SupervisorClassId">督学班级id</param>
/// <param name="RegisterUserId">注册用户id</param>
/// <param name="Name">昵称</param>
/// <param name="Phone">手机号</param>
public record CreateSupervisorStudentCommand(
    Guid SupervisorClassId,
    Guid RegisterUserId,
    string Name,
    string Phone
) : Command("创建督学学员")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {
        validator.RuleFor(x => Name)
                 .NotEmpty().WithMessage("昵称不能为空")
                 .MaximumLength(50).WithMessage("昵称长度不能大于50")
                 .MinimumLength(2).WithMessage("昵称长度不能小于2");

        validator.RuleFor(x => Phone)
                 .NotEmpty().WithMessage("手机号不能为空")
                 .MaximumLength(50).WithMessage("手机号长度不能大于50")
                 .MinimumLength(2).WithMessage("手机号长度不能小于2");
    }
}