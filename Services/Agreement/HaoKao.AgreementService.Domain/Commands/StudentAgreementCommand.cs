namespace HaoKao.AgreementService.Domain.Commands;

/// <summary>
/// 创建学员协议命令
/// </summary>
/// <param name="ProductId">产品Id</param>
/// <param name="ProductName">产品名称</param>
/// <param name="AgreementId">协议Id</param>
/// <param name="AgreementName">协议名称</param>
/// <param name="StudentName">学员名称</param>
/// <param name="IdCard">身份证号</param>
/// <param name="Contact">联系电话</param>
/// <param name="Address">联系地址</param>
/// <param name="Email">电子邮箱</param>
public record CreateStudentAgreementCommand(
    Guid ProductId,
    string ProductName,
    Guid AgreementId,
    string AgreementName,
    string StudentName,
    string IdCard,
    string Contact,
    string Address,
    string Email
) : Command("创建学员协议")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {
        validator.RuleFor(x => ProductName)
                 .NotEmpty().WithMessage("产品名称不能为空")
                 .MaximumLength(50).WithMessage("产品名称长度不能大于50");

        validator.RuleFor(x => AgreementName)
                 .NotEmpty().WithMessage("协议名称不能为空")
                 .MaximumLength(50).WithMessage("协议名称长度不能大于50");

        validator.RuleFor(x => StudentName)
                 .NotEmpty().WithMessage("学员名称不能为空")
                 .MaximumLength(50).WithMessage("学员名称长度不能大于50");

        validator.RuleFor(x => IdCard)
                 .NotEmpty().WithMessage("身份证号不能为空")
                 .MaximumLength(18).WithMessage("身份证号长度不能大于18");
    }
}

/// <summary>
/// 更新学员协议命令
/// </summary>
/// <param name="Id">主键</param>
/// <param name="StudentName">学员名称</param>
/// <param name="IdCard">身份证号</param>
/// <param name="Contact">联系电话</param>
/// <param name="Address">联系地址</param>
/// <param name="Email">电子邮箱</param>
public record UpdateStudentAgreementCommand(
    Guid Id,
    string StudentName,
    string IdCard,
    string Contact,
    string Address,
    string Email
) : Command("更新学员协议")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {
        validator.RuleFor(x => StudentName)
                 .NotEmpty().WithMessage("学员名称不能为空")
                 .MaximumLength(50).WithMessage("学员名称长度不能大于50");

        validator.RuleFor(x => IdCard)
                 .NotEmpty().WithMessage("身份证号不能为空")
                 .MaximumLength(18).WithMessage("身份证号长度不能大于18");
    }
}