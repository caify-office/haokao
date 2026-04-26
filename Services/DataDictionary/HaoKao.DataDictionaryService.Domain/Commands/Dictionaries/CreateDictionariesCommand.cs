using FluentValidation;

namespace HaoKao.DataDictionaryService.Domain.Commands.Dictionaries;

/// <summary>
/// 创建数据字典值
/// </summary>
/// <param name="Sort">排序</param>
/// <param name="Code">分组编码</param>
/// <param name="Name">值名称</param>
/// <param name="Pid">父节点id</param>
/// <param name="PName">父节点名称</param>
/// <param name="State">状态</param>
public record CreateDictionariesCommand(int Sort, string Code, string Name, Guid? Pid, string PName, bool State) : Command<(Guid?, string)>("创建数据字典值")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {
        validator.RuleFor(x => Code).MaximumLength(30).WithMessage("字典编码长度不能大于30");
        validator.RuleFor(x => Name).MaximumLength(225).WithMessage("名称长度不能大于225");
        validator.RuleFor(x => PName).MaximumLength(225).WithMessage("父节点名称长度不能大于225");
    }
}