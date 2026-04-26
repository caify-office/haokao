namespace HaoKao.ContinuationService.Domain.ContinuationAuditModule;

/// <summary>
/// 创建续读审核命令
/// </summary>
/// <param name="SetupId">续读配置Id</param>
/// <param name="ProductId">产品Id</param>
/// <param name="ProductName">产品名称</param>
/// <param name="AgreementId">协议Id</param>
/// <param name="AgreementName">协议名称</param>
/// <param name="ExpiryTime">产品过期时间</param>
/// <param name="StudentName">学员姓名</param>
/// <param name="Reason">续读原因</param>
/// <param name="RestOfCount">剩余申请次数</param>
/// <param name="Description">详细描述</param>
/// <param name="Evidences">相关证明</param>
/// <param name="ProductGifts">产品的赠品Id集合</param>
public record CreateContinuationAuditCommand(
    Guid SetupId,
    Guid ProductId,
    string ProductName,
    Guid AgreementId,
    string AgreementName,
    DateTime ExpiryTime,
    string StudentName,
    Guid Reason,
    int RestOfCount,
    string Description,
    List<string> Evidences,
    List<string> ProductGifts
) : Command("创建续读审核")
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
                 .NotEmpty().WithMessage("学员姓名不能为空")
                 .MaximumLength(50).WithMessage("学员姓名长度不能大于50");

        validator.RuleFor(x => Description)
                 .MaximumLength(500).WithMessage("详细描述长度不能大于500");

        validator.RuleFor(x => ExpiryTime)
                 .NotEmpty().WithMessage("产品过期时间不能为空");

        validator.RuleFor(x => Evidences)
                 .NotEmpty().WithMessage("相关证明不能为空");

        validator.RuleFor(x => RestOfCount)
                 .Must(x => x > 0).WithMessage("剩余申请次数要大于0");
    }
}

/// <summary>
/// 更新续读审核命令
/// </summary>
/// <param name="Id">主键</param>
/// <param name="AuditState">审核状态</param>
/// <param name="AuditReason">不通过原因</param>
public record UpdateContinuationAuditCommand(Guid Id, AuditState AuditState, string AuditReason) : Command("更新续读审核")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {
        if (AuditState == AuditState.NotPass)
        {
            validator.RuleFor(x => AuditReason)
                     .NotEmpty().WithMessage("不通过原因不能为空")
                     .MaximumLength(500).WithMessage("不通过原因长度不能大于500");
        }
    }
}