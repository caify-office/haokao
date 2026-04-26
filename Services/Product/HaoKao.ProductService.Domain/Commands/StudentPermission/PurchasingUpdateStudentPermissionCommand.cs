namespace HaoKao.ProductService.Domain.Commands.StudentPermission;

public record PurchasingUpdateStudentPermissionCommand(
    string StudentName,
    Guid StudentId,
    string OrderNumber,
    Dictionary<Guid, string> Products,
    DateTime ExpiryTime,
    bool Enable = true
) : Command("支付订单成功后，更新学员权限")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {
        validator.RuleFor(x => StudentName)
                 .NotEmpty().WithMessage("学员昵称(即用户昵称)不能为空")
                 .MaximumLength(50).WithMessage("学员昵称(即用户昵称)长度不能大于50")
                 .MinimumLength(2).WithMessage("学员昵称(即用户昵称)长度不能小于2");

        validator.RuleFor(x => OrderNumber)
                 .NotEmpty().WithMessage("对应的订单号不能为空")
                 .MaximumLength(50).WithMessage("对应的订单号长度不能大于50")
                 .MinimumLength(2).WithMessage("对应的订单号长度不能小于2");
    }
}