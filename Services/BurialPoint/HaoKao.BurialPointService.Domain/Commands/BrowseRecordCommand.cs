using HaoKao.BurialPointService.Domain.Enums;

namespace HaoKao.BurialPointService.Domain.Commands;

/// <summary>
/// 创建浏览记录命令
/// </summary>
/// <param name="BurialPointName">埋点名称</param>
/// <param name="BelongingPortType">所属端</param>
/// <param name="BrowseData">浏览信息</param>
/// <param name="IsPaidUser">是否付费用户</param>
public record CreateBrowseRecordCommand(
    string BurialPointName,
    BelongingPortType BelongingPortType,
    string BrowseData,
    bool IsPaidUser
) : Command("创建浏览记录")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {
        validator.RuleFor(x => BrowseData)
                 .NotEmpty().WithMessage("浏览信息不能为空");
    }
}