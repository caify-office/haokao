using System.Collections.Generic;

namespace HaoKao.LecturerService.Domain.Commands;

/// <summary>
/// 创建讲师命令
/// </summary>
/// <param name="Name">教师姓名</param>
/// <param name="SubjectIds">科目Id数组</param>
/// <param name="SubjectNames">科目名称数组</param>
/// <param name="Desc">简介</param>
/// <param name="CourseIntroduction">课程介绍</param>
/// <param name="HeadPortraitUrl">头像</param>
/// <param name="PhotoUrl">形象照</param>
/// <param name="Sort">排序</param>
/// <param name="ProductPackageIds"></param>
public record CreateLecturerCommand(
    string Name,
    List<Guid> SubjectIds,
    List<string> SubjectNames,
    string Desc,
    string CourseIntroduction,
    string HeadPortraitUrl,
    string PhotoUrl,
    int Sort,
    List<Guid> ProductPackageIds
) : Command("创建讲师")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {
        validator.RuleFor(x => Name)
                 .NotEmpty().WithMessage("教师姓名不能为空")
                 .MaximumLength(50).WithMessage("教师姓名长度不能大于50");

        validator.RuleFor(x => HeadPortraitUrl)
                 .NotEmpty().WithMessage("头像不能为空")
                 .MaximumLength(500).WithMessage("头像长度不能大于500");

        validator.RuleFor(x => PhotoUrl)
                 .NotEmpty().WithMessage("形象照不能为空")
                 .MaximumLength(500).WithMessage("形象照长度不能大于500");
    }
}

/// <summary>
/// 更新讲师命令
/// </summary>
/// <param name="Id">主键</param>
/// <param name="Name">教师姓名</param>
/// <param name="SubjectIds">科目Id数组</param>
/// <param name="SubjectNames">科目名称数组</param>
/// <param name="Desc">简介</param>
/// <param name="CourseIntroduction">课程介绍</param>
/// <param name="HeadPortraitUrl">头像</param>
/// <param name="PhotoUrl">形象照</param>
/// <param name="Sort">排序</param>
/// <param name="ProductPackageIds"></param>
public record UpdateLecturerCommand(
    Guid Id,
    string Name,
    List<Guid> SubjectIds,
    List<string> SubjectNames,
    string Desc,
    string CourseIntroduction,
    string HeadPortraitUrl,
    string PhotoUrl,
    int Sort,
    List<Guid> ProductPackageIds
) : Command("更新讲师")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {
        validator.RuleFor(x => Name)
                 .NotEmpty().WithMessage("教师姓名不能为空")
                 .MaximumLength(50).WithMessage("教师姓名长度不能大于50");

        validator.RuleFor(x => HeadPortraitUrl)
                 .NotEmpty().WithMessage("头像不能为空")
                 .MaximumLength(500).WithMessage("头像长度不能大于500");

        validator.RuleFor(x => PhotoUrl)
                 .NotEmpty().WithMessage("形象照不能为空")
                 .MaximumLength(500).WithMessage("形象照长度不能大于500");
    }
}

/// <summary>
/// 删除讲师命令
/// </summary>
/// <param name="Id">主键</param>
public record DeleteLecturerCommand(Guid Id) : Command("删除讲师");