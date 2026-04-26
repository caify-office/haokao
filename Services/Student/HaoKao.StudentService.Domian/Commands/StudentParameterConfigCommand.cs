namespace HaoKao.StudentService.Domain.Commands;

/// <summary>
/// 创建学员参数设置命令
/// </summary>
/// <param name="UserId">用户Id</param>
/// <param name="NickName">昵称</param>
/// <param name="PropertyName">设置值字段名称</param>
/// <param name="PropertyType">设置值类型</param>
/// <param name="PropertyValue">设置值</param>
/// <param name="Desc">描述</param>
public record CreateStudentParameterConfigCommand(
    Guid UserId,
    string NickName,
    string PropertyName,
    string PropertyType,
    string PropertyValue,
    string Desc
) : Command("创建学员参数设置")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {
        validator.RuleFor(x => NickName)
            .NotEmpty().WithMessage("昵称不能为空")
            .MaximumLength(50).WithMessage("昵称长度不能大于50")
            .MinimumLength(2).WithMessage("昵称长度不能小于2");

        validator.RuleFor(x => PropertyName)
            .NotEmpty().WithMessage("设置值字段名称不能为空")
            .MaximumLength(50).WithMessage("设置值字段名称长度不能大于50")
            .MinimumLength(2).WithMessage("设置值字段名称长度不能小于2");

        validator.RuleFor(x => PropertyType)
            .NotEmpty().WithMessage("设置值类型不能为空")
            .MaximumLength(500).WithMessage("设置值类型长度不能大于500")
            .MinimumLength(2).WithMessage("设置值类型长度不能小于2");

        validator.RuleFor(x => PropertyValue)
            .NotEmpty().WithMessage("设置值不能为空")
            .MaximumLength(500).WithMessage("设置值长度不能大于500");

        validator.RuleFor(x => Desc)
            .MaximumLength(1000).WithMessage("描述长度不能大于1000");
    }
}

/// <summary>
/// 更新学员参数设置命令
/// </summary>
/// <param name="Id">主键</param>
/// <param name="UserId">用户Id</param>
/// <param name="NickName">昵称</param>
/// <param name="PropertyName">设置值字段名称</param>
/// <param name="PropertyType">设置值类型</param>
/// <param name="PropertyValue">设置值</param>
/// <param name="Desc">描述</param>
public record UpdateStudentParameterConfigCommand(
   Guid Id,
   Guid UserId,
   string NickName,
   string PropertyName,
   string PropertyType,
   string PropertyValue,
   string Desc
) : Command("更新学员参数设置")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {
        validator.RuleFor(x => NickName)
           .NotEmpty().WithMessage("昵称不能为空")
           .MaximumLength(50).WithMessage("昵称长度不能大于50")
           .MinimumLength(2).WithMessage("昵称长度不能小于2");

        validator.RuleFor(x => PropertyName)
            .NotEmpty().WithMessage("设置值字段名称不能为空")
            .MaximumLength(50).WithMessage("设置值字段名称长度不能大于50")
            .MinimumLength(2).WithMessage("设置值字段名称长度不能小于2");

        validator.RuleFor(x => PropertyType)
            .NotEmpty().WithMessage("设置值类型不能为空")
            .MaximumLength(500).WithMessage("设置值类型长度不能大于500")
            .MinimumLength(2).WithMessage("设置值类型长度不能小于2");

        validator.RuleFor(x => PropertyValue)
            .NotEmpty().WithMessage("设置值不能为空")
            .MaximumLength(500).WithMessage("设置值长度不能大于500");

        validator.RuleFor(x => Desc)
            .MaximumLength(1000).WithMessage("描述长度不能大于1000");
    }
}

/// <summary>
/// 保存学员参数设置命令
/// </summary>
/// <param name="UserId">用户Id</param>
/// <param name="NickName">昵称</param>
/// <param name="PropertyName">设置值字段名称</param>
/// <param name="PropertyType">设置值类型</param>
/// <param name="PropertyValue">设置值</param>
/// <param name="Desc">描述</param>
public record SaveStudentParameterConfigCommand(
    Guid UserId,
    string NickName,
    string PropertyName,
    string PropertyType,
    string PropertyValue,
    string Desc
) : Command("保存学员参数设置")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {
        validator.RuleFor(x => NickName)
            .NotEmpty().WithMessage("昵称不能为空")
            .MaximumLength(50).WithMessage("昵称长度不能大于50")
            .MinimumLength(2).WithMessage("昵称长度不能小于2");

        validator.RuleFor(x => PropertyName)
            .NotEmpty().WithMessage("设置值字段名称不能为空")
            .MaximumLength(50).WithMessage("设置值字段名称长度不能大于50")
            .MinimumLength(2).WithMessage("设置值字段名称长度不能小于2");

        validator.RuleFor(x => PropertyType)
            .NotEmpty().WithMessage("设置值类型不能为空")
            .MaximumLength(500).WithMessage("设置值类型长度不能大于500")
            .MinimumLength(2).WithMessage("设置值类型长度不能小于2");

        validator.RuleFor(x => PropertyValue)
            .NotEmpty().WithMessage("设置值不能为空")
            .MaximumLength(500).WithMessage("设置值长度不能大于500");

        validator.RuleFor(x => Desc)
            .MaximumLength(1000).WithMessage("描述长度不能大于1000");
    }
}

/// <summary>
/// 删除学员参数设置命令
/// </summary>
/// <param name="Id">主键</param>
public record DeleteStudentParameterConfigCommand(Guid Id) : Command("删除学员参数设置");