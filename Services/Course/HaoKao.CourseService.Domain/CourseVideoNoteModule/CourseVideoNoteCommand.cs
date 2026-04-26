namespace HaoKao.CourseService.Domain.CourseVideoNoteModule;

/// <summary>
/// 创建课程视频笔记命令
/// </summary>
/// <param name="VideoId">视频id</param>
/// <param name="TimeNode">视频时间节点</param>
/// <param name="CourseVideoNoteType">笔记类型</param>
/// <param name="NoteContent">笔记内容</param>
public record CreateCourseVideoNoteCommand(
    string VideoId,
    decimal TimeNode,
    CourseVideoNoteType CourseVideoNoteType,
    string NoteContent
) : Command("创建课程视频笔记")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {
        validator.RuleFor(x => VideoId)
                 .NotEmpty().WithMessage("视频id不能为空")
                 .MaximumLength(50).WithMessage("视频id长度不能大于50")
                 .MinimumLength(2).WithMessage("视频id长度不能小于2");

        validator.RuleFor(x => NoteContent)
                 .NotEmpty().WithMessage("笔记内容不能为空")
                 .MaximumLength(500).WithMessage("笔记内容长度不能大于500")
                 .MinimumLength(2).WithMessage("笔记内容长度不能小于2");
    }
}

/// <summary>
/// 更新课程视频笔记命令
/// </summary>
/// <param name="Id">主键</param>
/// <param name="VideoId">视频id</param>
/// <param name="TimeNode">视频时间节点</param>
/// <param name="CourseVideoNoteType">笔记类型</param>
/// <param name="NoteContent">笔记内容</param>
public record UpdateCourseVideoNoteCommand(
    Guid Id,
    string VideoId,
    decimal TimeNode,
    CourseVideoNoteType CourseVideoNoteType,
    string NoteContent
) : Command("更新课程视频笔记")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {
        validator.RuleFor(x => VideoId)
                 .NotEmpty().WithMessage("视频id不能为空")
                 .MaximumLength(50).WithMessage("视频id长度不能大于50")
                 .MinimumLength(2).WithMessage("视频id长度不能小于2");

        validator.RuleFor(x => NoteContent)
                 .NotEmpty().WithMessage("笔记内容不能为空")
                 .MaximumLength(500).WithMessage("笔记内容长度不能大于500")
                 .MinimumLength(2).WithMessage("笔记内容长度不能小于2");
    }
}

/// <summary>
/// 删除课程视频笔记命令
/// </summary>
/// <param name="Id">主键</param>
public record DeleteCourseVideoNoteCommand(Guid Id) : Command("删除课程视频笔记");

/// <summary>
/// 批量删除课程视频笔记
/// </summary>
/// <param name="VideoId">视频id</param>
public record DeleteBatchCourseVideoNoteCommand(string VideoId) : Command("批量删除课程视频笔记");