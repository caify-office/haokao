namespace HaoKao.WebsiteConfigurationService.Domain.Commands.WebsiteTemplate;
/// <summary>
/// 设置模板内容
/// </summary>
/// <param name="Id">主键</param>
/// <param name="Content">内容</param>
public record SetWebsiteTemplateContentCommand(
   Guid Id,
   string Content
) : Command("设置模板内容")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {
        validator.RuleFor(x => Content)
            .NotEmpty().WithMessage("内容不能为空");
    }
}