namespace HaoKao.LiveBroadcastService.Domain.Commands.LiveAnnouncement;

/// <summary>
/// 创建直播公告命令
/// </summary>
/// <param name="Title">标题</param>
/// <param name="Content">内容</param>
public record CreateLiveAnnouncementCommand(
    string Title,
    string Content
) : Command("创建直播公告")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {
        validator.RuleFor(x => Title)
                 .NotEmpty().WithMessage("标题不能为空")
                 .MaximumLength(50).WithMessage("标题长度不能大于50")
                 .MinimumLength(2).WithMessage("标题长度不能小于2");


        validator.RuleFor(x => Content)
                 .NotEmpty().WithMessage("内容不能为空");
    }
}