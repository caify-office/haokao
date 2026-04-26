using HaoKao.CampaignService.Domain.Entities;
using HaoKao.CampaignService.Domain.Enums;
using HaoKao.CampaignService.Domain.ReceiveRules;

namespace HaoKao.CampaignService.Domain.Commands;

public record CreateGiftBagCommand(
    string CampaignName,
    GiftType GiftType,
    Guid ProductId,
    string ProductName,
    DateTime StartTime,
    DateTime EndTime,
    bool IsPublished,
    int Sort,
    GiftBagImageSet WebSiteImageSet,
    GiftBagImageSet WeChatMiniProgramImageSet,
    IReadOnlyList<Guid> ReceiveRules
) : Command("创建礼包命令")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {
        validator.RuleFor(x => CampaignName).NotEmpty().WithMessage("活动名称不能为空");

        validator.RuleFor(x => EndTime).Custom((endTime, context) =>
        {
            if (endTime < StartTime)
            {
                context.AddFailure(nameof(EndTime), "结束不能小于开始时间");
            }
        });

        validator.RuleForEach(x => ReceiveRules).Custom((rule, context) =>
        {
            var rules = EngineContext.Current.ResolveAll<IReceiveRule>();
            if (rules.All(r => r.RuleId != rule))
            {
                context.AddFailure(nameof(ReceiveRules), "领取规则不存在");
            }
        });
    }
}

public record UpdateGiftBagCommand(
    Guid Id,
    string CampaignName,
    GiftType GiftType,
    Guid ProductId,
    string ProductName,
    DateTime StartTime,
    DateTime EndTime,
    bool IsPublished,
    int Sort,
    GiftBagImageSet WebSiteImageSet,
    GiftBagImageSet WeChatMiniProgramImageSet,
    IReadOnlyList<Guid> ReceiveRules
) : Command("修改礼包命令")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {
        validator.RuleFor(x => CampaignName).NotEmpty().WithMessage("活动名称不能为空");

        validator.RuleFor(x => EndTime).Custom((endTime, context) =>
        {
            if (endTime < StartTime)
            {
                context.AddFailure(nameof(EndTime), "结束不能小于开始时间");
            }
        });

        validator.RuleForEach(x => ReceiveRules).Custom((rule, context) =>
        {
            var rules = EngineContext.Current.ResolveAll<IReceiveRule>();
            if (rules.All(r => r.RuleId != rule))
            {
                context.AddFailure(nameof(ReceiveRules), "领取规则不存在");
            }
        });
    }
}

public record UpdateGiftBagPublishedCommand(IReadOnlyList<Guid> Ids, bool IsPublished) : Command("修改礼品包发布状态命令");

public record ReceiveGiftBagCommand(Guid GiftBagId, Guid UserId, string UserName, DateTime RegistrationTime) : Command<GiftBag>("领取礼包命令");

public record DeleteGiftBagCommand(IReadOnlyList<Guid> Ids) : Command("删除礼品包命令");