using HaoKao.FeedBackService.Domain.Enums;

namespace HaoKao.FeedBackService.Domain.Commands.FeedBack;
/// <summary>
/// 创建命令
/// </summary>
/// <param name="Type">反馈类型</param>
/// <param name="SourceType">反馈来源</param>
/// <param name="Status">状态</param>
/// <param name="Contract">联系人</param>
/// <param name="Description">描述</param>
/// <param name="FileUrls">图片</param>
/// <param name="ParentId">父id</param>
public record CreateFeedBackCommand(
    TypeEnum Type,
    SourceTypeEnum SourceType,
    StatusEnum Status,
    string Contract,
    string Description,
    string FileUrls,Guid? ParentId
) : Command("创建")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {


        validator.RuleFor(x => Contract)
            .NotEmpty().WithMessage("联系人不能为空");

        validator.RuleFor(x => Description)
            .NotEmpty().WithMessage("描述不能为空");

        validator.RuleFor(x => FileUrls)
            .NotEmpty().WithMessage("图片不能为空");
      


    

    }
}