namespace HaoKao.SalespersonService.Domain.Commands;

/// <summary>
/// 创建企业微信配置命令
/// </summary>
/// <param name="CorpId">企业Id</param>
/// <param name="CorpName">企业名称</param>
/// <param name="CorpSecret">应用密钥凭证</param>
public record CreateEnterpriseWeChatConfigCommand(string CorpId, string CorpName, string CorpSecret) : Command("创建企业微信配置命令")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {
        validator.RuleFor(x => CorpId).NotEmpty().WithMessage("企业Id不能为空");
        validator.RuleFor(x => CorpName).NotEmpty().WithMessage("企业名称不能为空");
        validator.RuleFor(x => CorpSecret).NotEmpty().WithMessage("应用密钥凭证不能为空");
    }
}

/// <summary>
/// 更新企业微信配置命令
/// </summary>
/// <param name="Id"></param>
/// <param name="CorpId">企业Id</param>
/// <param name="CorpName">企业名称</param>
/// <param name="CorpSecret">应用密钥凭证</param>
public record UpdateEnterpriseWeChatConfigCommand(Guid Id, string CorpId, string CorpName, string CorpSecret) : Command("更新企业微信配置命令")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {
        validator.RuleFor(x => CorpId).NotEmpty().WithMessage("企业Id不能为空");
        validator.RuleFor(x => CorpName).NotEmpty().WithMessage("企业名称不能为空");
        validator.RuleFor(x => CorpSecret).NotEmpty().WithMessage("应用密钥凭证不能为空");
    }
}

/// <summary>
/// 删除企业微信配置命令
/// </summary>
/// <param name="Ids"></param>
public record DeleteEnterpriseWeChatConfigCommand(IReadOnlyList<Guid> Ids) : Command("删除企业微信配置命令");