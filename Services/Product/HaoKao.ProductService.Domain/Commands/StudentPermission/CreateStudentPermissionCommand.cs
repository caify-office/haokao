namespace HaoKao.ProductService.Domain.Commands.StudentPermission;

/// <summary>
/// 创建学员权限表命令
/// </summary>
/// <param name="StudentName">学员昵称(即用户昵称)</param>
/// <param name="StudentId">学员ID（即用户ID）</param>
/// <param name="OrderNumber">对应的订单号</param>
/// <param name="ProductId">产品Id</param>
/// <param name="ProductName">产品名称</param>
/// <param name="ExpiryTime">到期时间</param>
/// <param name="Enable">启用/禁用</param>
public record CreateStudentPermissionCommand(
    string StudentName,
    Guid StudentId,
    string OrderNumber,
    Guid ProductId,
    string ProductName,
    DateTime ExpiryTime,
    bool Enable = true
) : Command("创建学员权限表")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {
        validator.RuleFor(x => StudentName)
                 .NotEmpty().WithMessage("学员昵称(即用户昵称)不能为空")
                 .MaximumLength(50).WithMessage("学员昵称(即用户昵称)长度不能大于50");


        validator.RuleFor(x => OrderNumber)
                 .MaximumLength(50).WithMessage("对应的订单号长度不能大于50");


        validator.RuleFor(x => ProductName)
                 .NotEmpty().WithMessage("产品名称不能为空")
                 .MaximumLength(150).WithMessage("产品名称长度不能大于150")
                 .MinimumLength(2).WithMessage("产品名称长度不能小于2");
    }
}