namespace HaoKao.LiveBroadcastService.Domain.Commands.LiveAnnouncement;

/// <summary>
/// 更新直播公告命令
/// </summary>
/// <param name="Id">主键</param>
/// <param name="Title">标题</param>
/// <param name="Content">内容</param>
public record UpdateLiveAnnouncementCommand(
    Guid Id,
    string Title,
    string Content
) : Command("更新直播公告")
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