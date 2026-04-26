using HaoKao.LiveBroadcastService.Domain.Enums;

namespace HaoKao.LiveBroadcastService.Domain.Commands.LiveMessage;

/// <summary>
/// 创建直播消息命令
/// </summary>
/// <param name="Id">消息Id</param>
/// <param name="Content">消息内容</param>
/// <param name="LiveMessageType">消息类型</param>
/// <param name="LiveId">直播间Id</param>
public record CreateLiveMessageCommand(
    Guid Id,
    string Content,
    LiveMessageType LiveMessageType,
    Guid LiveId
) : Command("创建直播消息")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {
        validator.RuleFor(x => Content)
                 .NotEmpty().WithMessage("内容不能为空");
    }
}