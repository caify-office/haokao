namespace ShortUrlService.Domain.Commands;

public record CreateRegisterAppCommand(
    string AppName,
    string AppCode,
    string? Description,
    IReadOnlyList<string> AppDomains
) : Command<string>("创建注册应用")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {
        validator.RuleFor(x => AppName).NotEmpty().WithMessage("AppName 不能为空");
        validator.RuleFor(x => AppCode).NotEmpty().WithMessage("AppCode 不能为空");
        validator.RuleForEach(x => AppDomains).Custom((domain, context) =>
        {
            if (!Uri.TryCreate(domain, UriKind.RelativeOrAbsolute, out _))
            {
                context.AddFailure(nameof(AppDomains), $"AppDomains {domain} 不是有效的 Url");
            }
        });
    }
}