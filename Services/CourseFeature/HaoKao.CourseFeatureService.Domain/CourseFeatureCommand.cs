namespace HaoKao.CourseFeatureService.Domain;

/// <summary>
/// 创建课程特色服务命令
/// </summary>
/// <param name="Name">服务名称</param>
/// <param name="Content">服务内容</param>
/// <param name="IconUrl">图标地址</param>
/// <param name="Enable">启用/禁用</param>
public record CreateCourseFeatureCommand(
    string Name,
    string Content,
    string IconUrl,
    bool Enable
) : Command("创建课程特色服务")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {
        validator.RuleFor(x => Name)
                 .NotEmpty().WithMessage("服务名称不能为空")
                 .MaximumLength(50).WithMessage("服务名称长度不能大于50");

        validator.RuleFor(x => Content)
                 .NotEmpty().WithMessage("服务内容不能为空")
                 .MaximumLength(500).WithMessage("服务内容长度不能大于500");

        validator.RuleFor(x => IconUrl)
                 .NotEmpty().WithMessage("图标地址不能为空")
                 .MaximumLength(500).WithMessage("图标地址长度不能大于500");
    }
}

/// <summary>
/// 更新课程特色服务命令
/// </summary>
/// <param name="Id">主键</param>
/// <param name="Name">服务名称</param>
/// <param name="Content">服务内容</param>
/// <param name="IconUrl">图标地址</param>
/// <param name="Enable">启用/禁用</param>
public record UpdateCourseFeatureCommand(
    Guid Id,
    string Name,
    string Content,
    string IconUrl,
    bool Enable
) : Command("更新课程特色服务")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {
        validator.RuleFor(x => Name)
                 .NotEmpty().WithMessage("服务名称不能为空")
                 .MaximumLength(50).WithMessage("服务名称长度不能大于50");

        validator.RuleFor(x => Content)
                 .NotEmpty().WithMessage("服务内容不能为空")
                 .MaximumLength(500).WithMessage("服务内容长度不能大于500");

        validator.RuleFor(x => IconUrl)
                 .NotEmpty().WithMessage("图标地址不能为空")
                 .MaximumLength(500).WithMessage("图标地址长度不能大于500");
    }
}

/// <summary>
/// 批量启用/禁用课程特色服务
/// </summary>
/// <param name="Ids">Ids</param>
/// <param name="Enable">启用/禁用</param>
public record EnableCourseFeatureCommand(List<Guid> Ids, bool Enable) : Command("批量启用/禁用课程特色服务");

/// <summary>
/// 批量删除课程特色服务命令
/// </summary>
/// <param name="Ids">Ids</param>
public record DeleteCourseFeatureCommand(List<Guid> Ids) : Command("批量删除课程特色服务");