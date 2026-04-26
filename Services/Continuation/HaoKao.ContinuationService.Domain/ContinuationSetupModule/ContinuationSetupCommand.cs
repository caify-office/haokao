namespace HaoKao.ContinuationService.Domain.ContinuationSetupModule;

/// <summary>
/// 创建续读配置命令
/// </summary>
/// <param name="StartTime">续读申请开始时间</param>
/// <param name="EndTime">续读申请结束时间</param>
/// <param name="ExpiryTime">续读后的到期时间</param>
/// <param name="Products">产品集合</param>
/// <param name="Enable">是否启用</param>
public record CreateContinuationSetupCommand(
    DateTime StartTime,
    DateTime EndTime,
    DateTime ExpiryTime,
    IReadOnlyList<SetupProduct> Products,
    bool Enable
) : Command("创建续读配置")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {
        validator.RuleFor(x => ExpiryTime)
                 .Must(x => x >= DateTime.Today).WithMessage("续读结束时间不能早于当前系统时间");

        validator.RuleFor(x => StartTime)
                 .Must(x => x >= DateTime.Today).WithMessage("续读申请开始时间不能早于当前系统时间");

        validator.RuleFor(x => EndTime)
                 .Must(x => x >= DateTime.Today).WithMessage("续读申请结束时间不能早于当前系统时间")
                 .Must(x => x >= StartTime).WithMessage("开始时间不能大于结束时间");
    }
}

/// <summary>
/// 更新续读配置命令
/// </summary>
/// <param name="Id">主键</param>
/// <param name="StartTime">续读申请开始时间</param>
/// <param name="EndTime">续读申请结束时间</param>
/// <param name="ExpiryTime">续读后的到期时间</param>
/// <param name="Products">产品集合</param>
/// <param name="Enable">是否启用</param>
public record UpdateContinuationSetupCommand(
    Guid Id,
    DateTime StartTime,
    DateTime EndTime,
    DateTime ExpiryTime,
    IReadOnlyList<SetupProduct> Products,
    bool Enable
) : Command("更新续读配置")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {
        validator.RuleFor(x => ExpiryTime)
                 .Must(x => x >= DateTime.Today).WithMessage("续读结束时间不能早于当前系统时间");

        validator.RuleFor(x => EndTime)
                 .Must(x => x >= DateTime.Today).WithMessage("续读申请结束时间不能早于当前系统时间")
                 .Must(x => x >= StartTime).WithMessage("开始时间不能大于结束时间");
    }
}

/// <summary>
/// 设置续读后会员到期时间
/// </summary>
/// <param name="Id">Id</param>
/// <param name="ExpiryTime">会员到期时间</param>
public record UpdateExpiryTimeCommand(Guid Id, DateTime ExpiryTime) : Command("设置续读后会员到期时间")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {
        validator.RuleFor(x => ExpiryTime)
                 .Must(x => x >= DateTime.Now).WithMessage("续读结束时间不能早于当前系统时间");
    }
}

/// <summary>
/// 启用/禁用续读配置
/// </summary>
/// <param name="Id">Id</param>
/// <param name="Enable">启用/禁用</param>
public record EnableContinuationSetupCommand(Guid Id, bool Enable) : Command("启用/禁用续读配置");

/// <summary>
/// 删除续读配置命令
/// </summary>
/// <param name="Id">主键</param>
public record DeleteContinuationSetupCommand(Guid Id) : Command("删除续读配置");