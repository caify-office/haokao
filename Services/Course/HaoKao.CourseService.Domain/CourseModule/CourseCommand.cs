namespace HaoKao.CourseService.Domain.CourseModule;

/// <summary>
/// 创建课程命令
/// </summary>
/// <param name="Name">课程名称</param>
/// <param name="TeacherJson">主讲老师ids集合</param>
/// <param name="Year">年份</param>
/// <param name="SubjectId">科目id</param>
/// <param name="SubjectName">科目名称</param>
/// <param name="State">启用/禁用</param>
/// <param name="CourseType">课程类型</param>
/// <param name="UpdateTimeDesc">更新时间</param>
public record CreateCourseCommand(
    string Name,
    string TeacherJson,
    string Year,
    Guid SubjectId,
    string SubjectName,
    bool State,
    CourseType CourseType,
    string UpdateTimeDesc
) : Command("创建课程")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {
        validator.RuleFor(x => Name).NotEmpty().WithMessage("课程名称不能为空");
        validator.RuleFor(x => TeacherJson).NotEmpty().WithMessage("主讲老师TeacherJson不能为空");
        validator.RuleFor(x => Year).NotEmpty().WithMessage("年份不能为空");
        validator.RuleFor(x => SubjectName).NotEmpty().WithMessage("科目名称不能为空");
    }
}

/// <summary>
/// 更新课程命令
/// </summary>
/// <param name="Id">主键</param>
/// <param name="Name">课程名称</param>
/// <param name="TeacherJson">主讲老师TeacherJson集合</param>
/// <param name="Year">年份</param>
/// <param name="SubjectId">科目id</param>
/// <param name="SubjectName">科目名称</param>
/// <param name="State">启用/禁用</param>
/// <param name="UpdateTimeDesc">预计更新时间</param>
public record UpdateCourseCommand(
    Guid Id,
    string Name,
    string TeacherJson,
    string Year,
    Guid SubjectId,
    string SubjectName,
    bool State,
    string UpdateTimeDesc
) : Command("更新课程")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {
        validator.RuleFor(x => Name).NotEmpty().WithMessage("课程名称不能为空");
        validator.RuleFor(x => TeacherJson).NotEmpty().WithMessage("主讲老师TeacherJson不能为空");
        validator.RuleFor(x => Year).NotEmpty().WithMessage("年份不能为空");
        validator.RuleFor(x => SubjectName).NotEmpty().WithMessage("科目名称不能为空");
    }
}

/// <summary>
/// 更新课程讲义包
/// </summary>
/// <param name="Id">主键</param>
/// <param name="CourseMaterialsPackageUrl">课程讲义包url</param>
/// <param name="CourseMaterialsPackageName">课程讲义包名称</param>
public record UpdateCourseMaterialsPackageUrlCommand(
    Guid Id,
    string CourseMaterialsPackageUrl,
    string CourseMaterialsPackageName
) : Command("更新课程讲义包");

/// <summary>
/// 启用/禁用修改命令
/// </summary>
/// <param name="Ids"></param>
/// <param name="State"></param>
public record UpdateEnableStateCommand(IEnumerable<Guid> Ids, bool State) : Command("启用/禁用修改");

/// <summary>
/// 删除课程命令
/// </summary>
/// <param name="Id">主键</param>
public record DeleteCourseCommand(Guid Id) : Command("删除课程");