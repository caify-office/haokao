namespace HaoKao.CourseService.Domain.VideoStorageModule;

/// <summary>
/// 保存视频存储配置
/// </summary>
/// <param name="VideoStorageHandlerId">视频存储器ID</param>
/// <param name="VideoStorageHandlerName">视频存储器名称</param>
/// <param name="ConfigParameter">相关的配置参数</param>
public record SaveVideoStorageCommand(
    Guid VideoStorageHandlerId,
    string VideoStorageHandlerName,
    string ConfigParameter
) : Command("保存视频存储配置命令")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {
        validator.RuleFor(x => VideoStorageHandlerName)
                 .NotEmpty().WithMessage("视频存储器名称不能为空")
                 .MaximumLength(50).WithMessage("视频存储器名称长度不能大于50")
                 .MinimumLength(2).WithMessage("视频存储器名称长度不能小于2");
    }
}