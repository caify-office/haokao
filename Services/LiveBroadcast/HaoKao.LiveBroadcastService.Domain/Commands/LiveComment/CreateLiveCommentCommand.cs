namespace HaoKao.LiveBroadcastService.Domain.Commands.LiveComment;

/// <summary>
/// 创建直播评论命令
/// </summary>
/// <param name="Phone">手机号</param>
/// <param name="Rating">评分</param>
/// <param name="Content">评价内容</param>
/// <param name="LiveId">直播Id</param>
public record CreateLiveCommentCommand(
    string Phone,
    int Rating,
    string Content,
    Guid LiveId
) : Command("创建直播评论")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {
        validator.RuleFor(x => Phone)
                 .NotEmpty().WithMessage("手机号不能为空")
                 .MaximumLength(50).WithMessage("手机号长度不能大于50")
                 .MinimumLength(2).WithMessage("手机号长度不能小于2");

        validator.RuleFor(x => Content)
                 .NotEmpty().WithMessage("评价内容不能为空")
                 .MaximumLength(300).WithMessage("评价内容长度不能大于300")
                 .MinimumLength(2).WithMessage("评价内容长度不能小于2");
    }
}