namespace HaoKao.CourseService.Domain.CourseChapterModule;

/// <summary>
/// 创建课程章节命令
/// </summary>
/// <param name="Name">名称</param>
/// <param name="ParentId">父id</param>
/// <param name="CourseId">关联的课程id</param>
/// <param name="IsLeaf">是否叶子节点</param>
/// <param name="Sort">排序</param>
public record CreateCourseChapterCommand(
    string Name,
    Guid ParentId,
    Guid CourseId,
    bool IsLeaf,
    int Sort
) : Command("创建课程章节")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {
        validator.RuleFor(x => Name).NotEmpty().WithMessage("名称不能为空");
    }
}

/// <summary>
/// 批量增加
/// </summary>
/// <param name="List">课程章节集合</param>
public record CreateCourseChapterBatchCommand(List<CourseChapter> List) : Command("批量增加");

/// <summary>
/// 更新课程章节命令
/// </summary>
/// <param name="Id">主键</param>
/// <param name="Name">名称</param>
/// <param name="ParentId">父id</param>
/// <param name="CourseId">关联的课程id</param>
/// <param name="IsLeaf">IsLeaf是否叶子节点</param>
/// <param name="Sort">排序</param>
public record UpdateCourseChapterCommand(
    Guid Id,
    string Name,
    Guid ParentId,
    Guid CourseId,
    bool IsLeaf,
    int Sort
) : Command("更新课程章节")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {
        validator.RuleFor(x => Name).NotEmpty().WithMessage("名称不能为空");
    }
}

/// <summary>
/// 删除课程章节命令
/// </summary>
/// <param name="Id">主键</param>
public record DeleteCourseChapterCommand(Guid Id) : Command("删除课程章节");

/// <summary>
/// 批量删除课程章节
/// </summary>
/// <param name="Id">主键id</param>
public record BatchDeleteCourseChapterCommand(Guid Id) : Command("批量删除课程章节");