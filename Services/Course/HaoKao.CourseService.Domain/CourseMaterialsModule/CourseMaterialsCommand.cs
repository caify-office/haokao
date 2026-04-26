namespace HaoKao.CourseService.Domain.CourseMaterialsModule;

/// <summary>
/// 创建课程讲义命令
/// </summary>
/// <param name="Name">讲义名称</param>
/// <param name="FileUrl">讲义地址</param>
/// <param name="CourseChapterId">课程章节id（阶段学习为课程章节id,智慧辅助学习为课程Id）</param>
public record CreateCourseMaterialsCommand(string Name, string FileUrl, Guid CourseChapterId) : Command("创建课程讲义")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {
        validator.RuleFor(x => Name).NotEmpty().WithMessage("讲义名称不能为空");
        validator.RuleFor(x => FileUrl).NotEmpty().WithMessage("讲义地址不能为空");
    }
}

/// <summary>
/// 保存课程讲义命令（智辅学习使用）
/// </summary>
/// <param name="Name">讲义名称</param>
/// <param name="FileUrl">讲义地址</param>
/// <param name="CourseChapterId">课程章节id（阶段学习为课程章节id,智慧辅助学习为课程Id）</param>
/// <param name="KnowledgePointId">知识点Id</param>
public record SaveCourseMaterialsCommand(string Name, string FileUrl, Guid CourseChapterId, Guid KnowledgePointId) : Command("保存课程讲义命令")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {
        validator.RuleFor(x => Name).NotEmpty().WithMessage("讲义名称不能为空");
        validator.RuleFor(x => FileUrl).NotEmpty().WithMessage("讲义地址不能为空");
        validator.RuleFor(x => KnowledgePointId).NotNull().WithMessage("知识点不能为空");
    }
}

/// <summary>
/// 设置课程讲义排序命令
/// </summary>
/// <param name="Id">主键</param>
/// <param name="Sort">排序</param>
public record SetCourseMaterialsSortCommand(Guid Id, int Sort) : Command("设置课程讲义排序命令");

/// <summary>
/// 删除课程讲义命令
/// </summary>
/// <param name="Id">主键</param>
public record DeleteCourseMaterialsCommand(Guid Id) : Command("删除课程讲义");