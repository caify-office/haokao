using HaoKao.CourseRatingService.Domain.Enums;

namespace HaoKao.CourseRatingService.Domain.Commands;

/// <summary>
/// 创建课程评价命令
/// </summary>
/// <param name="CourseId">课程Id</param>
/// <param name="CourseName">课程名称</param>
/// <param name="Comment">评价内容</param>
/// <param name="Rating">评价级别</param>
/// <param name="NickName">昵称</param>
/// <param name="Avatar">头像</param>
public record CreateCourseRatingCommand(
    Guid CourseId,
    string CourseName,
    string Comment,
    int Rating,
    string NickName,
    string Avatar
) : Command("创建课程评价")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {
        validator.RuleFor(x => CourseName)
                 .NotEmpty().WithMessage("课程名称不能为空")
                 .MaximumLength(50).WithMessage("课程名称长度不能大于50");

        validator.RuleFor(x => Comment)
                 .NotEmpty().WithMessage("评价内容不能为空")
                 .MaximumLength(300).WithMessage("评价内容长度不能大于300");

        validator.RuleFor(x => NickName)
                 .NotEmpty().WithMessage("昵称不能为空")
                 .MaximumLength(50).WithMessage("昵称长度不能大于50");

        validator.RuleFor(x => Avatar)
                 .NotEmpty().WithMessage("头像不能为空");
    }
}

/// <summary>
/// 审核课程评价
/// </summary>
/// <param name="Id">主键</param>
/// <param name="AuditState">审核状态</param>
public record AuditCourseRatingCommand(Guid Id, AuditState AuditState) : Command("审核课程评价");

/// <summary>
/// 置顶课程评价
/// </summary>
/// <param name="Id">主键</param>
/// <param name="Sticky">是否置顶</param>
public record StickyCourseRatingCommand(Guid Id, bool Sticky) : Command("置顶课程评价");

/// <summary>
/// 删除课程评价命令
/// </summary>
/// <param name="Id">主键</param>
public record DeleteCourseRatingCommand(Guid Id) : Command("删除课程评价");