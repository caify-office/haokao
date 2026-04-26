namespace HaoKao.ContinuationService.Domain.ProductExtensionPolicyModule;

/// <summary>
/// 创建课程续读策略命令
/// </summary>
/// <param name="Name">策略名称</param>
/// <param name="StartTime">申请开始时间</param>
/// <param name="EndTime">申请结束时间</param>
/// <param name="ExtensionType">延期类型</param>
/// <param name="ExtensionDays">延长天数</param>
/// <param name="ExpiryDate">固定过期时间</param>
/// <param name="Products">产品集合</param>
/// <param name="IsEnable">是否启用</param>
public record CreateProductExtensionPolicyCommand(
    string Name,
    DateTime StartTime,
    DateTime EndTime,
    ExtensionType ExtensionType,
    int? ExtensionDays,
    DateTime? ExpiryDate,
    List<PolicyProduct> Products,
    bool IsEnable
) : Command("创建课程续读策略")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {
        validator.RuleFor(x => Name)
                 .NotEmpty().WithMessage("策略名称不能为空")
                 .MaximumLength(50).WithMessage("策略名称长度不能大于50");

        validator.RuleFor(x => StartTime)
                 .Must(x => x >= DateTime.Today).WithMessage("申请开始时间不能早于当前系统时间");

        validator.RuleFor(x => EndTime)
                 .Must(x => x >= DateTime.Today).WithMessage("申请结束时间不能早于当前系统时间")
                 .Must(x => x >= StartTime).WithMessage("开始时间不能大于结束时间");

        validator.RuleFor(x => ExtensionType)
                 .IsInEnum().WithMessage("无效的延期类型");

        // 根据 ExtensionType 验证
        validator.RuleFor(x => ExpiryDate)
                 .Must((cmd, date) =>
                 {
                     var command = cmd as CreateProductExtensionPolicyCommand;
                     return command.ExtensionType != ExtensionType.FixedDate || (date.HasValue && date.Value >= DateTime.Today);
                 })
                 .WithMessage("当选择固定日期延期时，必须指定有效的过期时间且不能早于今天");

        validator.RuleFor(x => ExtensionDays)
                 .Must((cmd, days) =>
                 {
                     var command = cmd as CreateProductExtensionPolicyCommand;
                     return command.ExtensionType != ExtensionType.Duration || (days.HasValue && days.Value > 0);
                 })
                 .WithMessage("当选择相对天数延期时，必须指定大于0的天数");
    }
}

/// <summary>
/// 更新课程续读策略命令
/// </summary>
/// <param name="Id">主键</param>
/// <param name="Name">策略名称</param>
/// <param name="StartTime">申请开始时间</param>
/// <param name="EndTime">申请结束时间</param>
/// <param name="ExtensionType">延期类型</param>
/// <param name="ExtensionDays">延长天数</param>
/// <param name="ExpiryDate">固定过期时间</param>
/// <param name="Products">产品集合</param>
/// <param name="IsEnable">是否启用</param>
public record UpdateProductExtensionPolicyCommand(
    Guid Id,
    string Name,
    DateTime StartTime,
    DateTime EndTime,
    ExtensionType ExtensionType,
    int? ExtensionDays,
    DateTime? ExpiryDate,
    List<PolicyProduct> Products,
    bool IsEnable
) : Command("更新课程续读策略")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {
        validator.RuleFor(x => Name)
                 .NotEmpty().WithMessage("策略名称不能为空")
                 .MaximumLength(50).WithMessage("策略名称长度不能大于50");

        validator.RuleFor(x => EndTime)
                 .Must(x => x >= DateTime.Today).WithMessage("申请结束时间不能早于当前系统时间")
                 .Must(x => x >= StartTime).WithMessage("开始时间不能大于结束时间");

        validator.RuleFor(x => ExtensionType)
                 .IsInEnum().WithMessage("无效的延期类型");

        validator.RuleFor(x => ExpiryDate)
                 .Must((cmd, date) =>
                 {
                     var command = cmd as UpdateProductExtensionPolicyCommand;
                     return command.ExtensionType != ExtensionType.FixedDate || (date.HasValue && date.Value >= DateTime.Today);
                 })
                 .WithMessage("当选择固定日期延期时，必须指定有效的过期时间且不能早于今天");

        validator.RuleFor(x => ExtensionDays)
                 .Must((cmd, days) =>
                 {
                     var command = cmd as UpdateProductExtensionPolicyCommand;
                     return command.ExtensionType != ExtensionType.Duration || (days.HasValue && days.Value > 0);
                 })
                 .WithMessage("当选择相对天数延期时，必须指定大于0的天数");
    }
}

/// <summary>
/// 删除课程续读策略命令
/// </summary>
/// <param name="Id">主键</param>
public record DeleteProductExtensionPolicyCommand(Guid Id) : Command("删除课程续读策略");

/// <summary>
/// 启用/禁用课程续读策略命令
/// </summary>
/// <param name="Id">主键</param>
/// <param name="IsEnable">启用/禁用</param>
public record EnableProductExtensionPolicyCommand(Guid Id, bool IsEnable) : Command("启用/禁用课程续读策略");