namespace HaoKao.NoticeService.Domain.Commands;

/// <summary>
/// 创建公告命令
/// </summary>
/// <param name="Title">公告名称</param>
/// <param name="Content">公告内容</param>
/// <param name="Popup">是否弹出</param>
/// <param name="StartTime">弹出开始时间</param>
/// <param name="EndTime">弹出结束时间</param>
/// <param name="Published">是否发布</param>
public record CreateNoticeCommand(
    string Title,
    string Content,
    bool Popup,
    DateTime? StartTime,
    DateTime? EndTime,
    bool Published
) : Command("创建公告")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {
        validator.RuleFor(x => Title)
                 .NotEmpty().WithMessage("公告名称不能为空")
                 .MaximumLength(50).WithMessage("公告名称长度不能大于50");

        validator.RuleFor(x => Content)
                 .NotEmpty().WithMessage("公告内容部能为空");

        if (Popup)
        {
            validator.RuleFor(x => StartTime).NotEmpty().WithMessage("公告弹出开始时间不能为空");
            validator.RuleFor(x => EndTime).NotEmpty().WithMessage("公告弹出结束时间不能为空");
        }
    }
}

/// <summary>
/// 更新公告命令
/// </summary>
/// <param name="Id">公告Id</param>
/// <param name="Title">公告名称</param>
/// <param name="Content">公告内容</param>
/// <param name="Popup">是否弹出</param>
/// <param name="StartTime">弹出开始时间</param>
/// <param name="EndTime">弹出结束时间</param>
/// <param name="Published">是否发布</param>
public record UpdateNoticeCommand(
    Guid Id,
    string Title,
    string Content,
    bool Popup,
    DateTime? StartTime,
    DateTime? EndTime,
    bool Published
) : Command("更新公告")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {
        validator.RuleFor(x => Title)
                 .NotEmpty().WithMessage("公告名称不能为空")
                 .MaximumLength(50).WithMessage("公告名称长度不能大于50");

        validator.RuleFor(x => Content)
                 .NotEmpty().WithMessage("公告内容部能为空");

        if (Popup)
        {
            validator.RuleFor(x => StartTime).NotEmpty().WithMessage("公告弹出开始时间不能为空");
            validator.RuleFor(x => EndTime).NotEmpty().WithMessage("公告弹出结束时间不能为空");
        }
    }
}

/// <summary>
/// 修改公告是否弹出
/// </summary>
/// <param name="Ids">公告Ids</param>
/// <param name="Popup">是否弹出</param>
/// <param name="StartTime">弹出开始时间</param>
/// <param name="EndTime">弹出结束时间</param>
public record UpdateNoticePopupCommand(
    List<Guid> Ids,
    bool Popup,
    DateTime? StartTime,
    DateTime? EndTime
) : Command("修改公告是否弹出")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {
        if (Popup)
        {
            validator.RuleFor(x => StartTime).NotEmpty().WithMessage("公告弹出开始时间不能为空");
            validator.RuleFor(x => EndTime).NotEmpty().WithMessage("公告弹出结束时间不能为空");
        }
    }
}

/// <summary>
/// 修改公告是否发布
/// </summary>
/// <param name="Ids">公告Ids</param>
/// <param name="Published">是否发布</param>
public record UpdateNoticePublishedCommand(List<Guid> Ids, bool Published) : Command("修改公告是否发布");

/// <summary>
/// 删除公告
/// </summary>
/// <param name="Ids">公告Ids</param>
public record DeleteNoticeCommand(List<Guid> Ids) : Command("删除公告");