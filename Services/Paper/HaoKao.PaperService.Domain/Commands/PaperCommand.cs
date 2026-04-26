using FluentValidation;
using HaoKao.PaperService.Domain.Enumerations;

namespace HaoKao.PaperService.Domain.Commands;

/// <summary>
/// 创建试卷命令
/// </summary>
/// <param name="Name">试卷名称</param>
/// <param name="SubjectId">科目id</param>
/// <param name="SubjectName">科目名称</param>
/// <param name="CategoryId">所属分类</param>
/// <param name="CategoryName">分类名称</param>
/// <param name="Time">考试时长</param>
/// <param name="Score">及格分数</param>
/// <param name="IsFree">是否限免  1--不限免 2--限免</param>
/// <param name="State">发布状态 1-未发布 2-已发布</param>
/// <param name="Sort">排序值</param>
/// <param name="Year">年份</param>
public record CreatePaperCommand(
    string Name,
    Guid SubjectId,
    string SubjectName,
    Guid CategoryId,
    string CategoryName,
    int Time,
    decimal Score,
    FreeEnum IsFree,
    StateEnum State,
    int Sort,
    int Year
) : Command("创建试卷")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {
        validator.RuleFor(x => Name)
                 .NotEmpty().WithMessage("试卷名称不能为空")
                 .MaximumLength(50).WithMessage("试卷名称长度不能大于50");

        validator.RuleFor(x => SubjectName)
                 .NotEmpty().WithMessage("科目名称不能为空")
                 .MaximumLength(50).WithMessage("科目名称长度不能大于50");

        validator.RuleFor(x => CategoryName)
                 .NotEmpty().WithMessage("分类名称不能为空")
                 .MaximumLength(50).WithMessage("分类名称长度不能大于50");
    }
}

/// <summary>
/// 更新试卷命令
/// </summary>
/// <param name="Id">主键</param>
/// <param name="Name">试卷名称</param>
/// <param name="SubjectId">科目id</param>
/// <param name="SubjectName">科目名称</param>
/// <param name="CategoryId">所属分类</param>
/// <param name="CategoryName">分类名称</param>
/// <param name="Time">考试时长</param>
/// <param name="Score">及格分数</param>
/// <param name="IsFree">是否限免  1--不限免 2--限免</param>
/// <param name="State">发布状态 1-未发布 2-已发布</param>
/// <param name="Sort">排序值</param>
/// <param name="Year">年份</param>
public record UpdatePaperCommand(
    Guid Id,
    string Name,
    Guid SubjectId,
    string SubjectName,
    Guid CategoryId,
    string CategoryName,
    int Time,
    decimal Score,
    FreeEnum IsFree,
    StateEnum State,
    int Sort,
    int Year
) : CreatePaperCommand(Name, SubjectId, SubjectName, CategoryId, CategoryName, Time, Score, IsFree, State, Sort, Year);

/// <summary>
/// 批量修改试卷发布状态
/// </summary>
/// <param name="Ids">试卷Ids</param>
/// <param name="PublishState">发布状态枚举</param>
public record UpdatePublishStateCommand(IEnumerable<Guid> Ids, StateEnum PublishState) : Command("批量修改试卷发布状态");

/// <summary>
/// 批量修改是否限免
/// </summary>
/// <param name="Ids">试卷Ids</param>
/// <param name="IsFree">限免枚举</param>
public record UpdateIsFreeCommand(IEnumerable<Guid> Ids, FreeEnum IsFree) : Command("批量修改是否限免");

/// <summary>
/// 设置试卷试题
/// </summary>
/// <param name="Id"></param>
/// <param name="StructJson"></param>
/// <param name="QuestionCount"></param>
/// <param name="TotalScore"></param>
public record UpdatePaperStructCommand(
    Guid Id,
    string StructJson,
    int QuestionCount,
    decimal TotalScore
) : Command("更新试卷")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {
        validator.RuleFor(x => StructJson)
                 .NotEmpty().WithMessage("试卷试题json不能为空");
    }
}

/// <summary>
/// 删除试卷命令
/// </summary>
/// <param name="Id">主键</param>
public record DeletePaperCommand(Guid Id) : Command("删除试卷");