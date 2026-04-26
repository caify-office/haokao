namespace HaoKao.ContinuationService.Domain.ProductExtensionRequestModule;

/// <summary>
/// 创建课程续读申请命令
/// </summary>
/// <param name="PolicyId">策略Id</param>
/// <param name="ProductId">产品Id</param>
/// <param name="ProductName">产品名称</param>
/// <param name="AgreementId">协议Id</param>
/// <param name="AgreementName">协议名称</param>
/// <param name="ExpiryTime">原产品过期时间</param>
/// <param name="StudentName">学员姓名</param>
/// <param name="ReasonId">续读原因Id</param>
/// <param name="RestOfCount">剩余申请次数</param>
/// <param name="Description">详细描述</param>
/// <param name="Evidences">相关证明</param>
/// <param name="ProductGifts">产品的赠品Id集合</param>
public record CreateProductExtensionRequestCommand(
    Guid PolicyId,
    Guid ProductId,
    string ProductName,
    Guid AgreementId,
    string AgreementName,
    DateTime ExpiryTime,
    string StudentName,
    Guid ReasonId,
    int RestOfCount,
    string Description,
    List<string> Evidences,
    List<string> ProductGifts
) : Command("创建课程续读申请")
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
/// 更新课程续读申请状态命令 (审核)
/// </summary>
/// <param name="Id">申请Id</param>
/// <param name="State">审核状态</param>
/// <param name="Remark">审核意见/理由</param>
public record UpdateProductExtensionRequestStateCommand(
    Guid Id,
    ProductExtensionRequestState State,
    string Remark
) : Command("更新课程续读申请状态")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {
        // 如果不是通过，则必须填写理由
        if (State is ProductExtensionRequestState.Rejected or ProductExtensionRequestState.NeedMoreEvidence)
        {
            validator.RuleFor(x => Remark)
                     .NotEmpty().WithMessage("审核意见不能为空")
                     .MaximumLength(500).WithMessage("审核意见长度不能大于500");
        }
    }
}