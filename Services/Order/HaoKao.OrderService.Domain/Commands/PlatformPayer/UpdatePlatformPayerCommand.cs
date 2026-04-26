using HaoKao.OrderService.Domain.Enums;

namespace HaoKao.OrderService.Domain.Commands.PlatformPayer;

/// <summary>
/// 更新平台配置的支付列表命令
/// </summary>
/// <param name="Id">主键</param>
/// <param name="Name">支付名称</param>
/// <param name="PayerId">对应支付处理者Id</param>
/// <param name="PayerName">对应支付处理者名称</param>
/// <param name="PaymentMethod">支付方式</param>
/// <param name="IosIsOpen">Ios是否开启</param>
/// <param name="PlatformPayerScenes">使用场景</param>
/// <param name="Config">支付相关配置</param>
public record UpdatePlatformPayerCommand(
   Guid Id,
   string Name,
   Guid PayerId,
   string PayerName,
   PaymentMethod PaymentMethod,
   bool IosIsOpen,
   PlatformPayerScenes PlatformPayerScenes,
   Dictionary<string, string> Config
) : Command("更新平台配置的支付列表")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {
        validator.RuleFor(x => Name)
            .NotEmpty().WithMessage("支付名称不能为空")
            .MaximumLength(50).WithMessage("支付名称长度不能大于50")
            .MinimumLength(2).WithMessage("支付名称长度不能小于2");

        validator.RuleFor(x => PayerId)
            .NotEmpty().WithMessage("对应支付处理者Id不能为空");

        validator.RuleFor(x => PayerName)
            .NotEmpty().WithMessage("对应支付处理者名称不能为空")
            .MaximumLength(50).WithMessage("对应支付处理者名称长度不能大于50")
            .MinimumLength(2).WithMessage("对应支付处理者名称长度不能小于2");
    }
}