using ShortUrlService.Domain.Entities;

namespace ShortUrlService.Domain.Commands;

public record CreateShortUrlCommand(
    long RegisterAppId,
    string OriginUrl,
    string ShortKey,
    int AccessLimit,
    DateTime ExpiredTime
) : Command<ShortUrl>("创建短链接命令")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {
        validator.RuleFor(x => AccessLimit).GreaterThan(0).WithMessage("AccessLimit 必须大于0");

        validator.RuleFor(x => ExpiredTime).Custom((expiredTime, context) =>
        {
            if (expiredTime < DateTime.Now)
            {
                context.AddFailure(nameof(ExpiredTime), "ExpiredTime 不能小于当前时间");
            }

            if (expiredTime < DateTime.Now.AddMinutes(5))
            {
                context.AddFailure(nameof(ExpiredTime), "ExpiredTime 不能少于5分钟");
            }
        });

        validator.RuleFor(x => OriginUrl).Custom((url, context) =>
        {
            if (!Uri.TryCreate(url, UriKind.Absolute, out var uri)
             && (uri?.Scheme == Uri.UriSchemeHttp || uri?.Scheme == Uri.UriSchemeHttps))
            {
                context.AddFailure(nameof(OriginUrl), $"OriginUrl {url} 不是有效的 Url");
            }
        });
    }
}

public record UpdateShortUrlCommand(long Id, int AccessLimit, DateTime ExpiredTime) : Command("更新短链接命令")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {
        validator.RuleFor(x => AccessLimit).GreaterThan(0).WithMessage("AccessLimit 必须大于0");

        validator.RuleFor(x => ExpiredTime).Custom((expiredTime, context) =>
        {
            if (expiredTime < DateTime.Now)
            {
                context.AddFailure(nameof(ExpiredTime), "ExpiredTime 不能小于当前时间");
            }

            if (expiredTime < DateTime.Now.AddMinutes(5))
            {
                context.AddFailure(nameof(ExpiredTime), "ExpiredTime 不能少于5分钟");
            }
        });
    }
}

public record UpdateShortKeyCommand(long Id, string ShortKey) : Command("更新ShortKey命令")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {
        validator.RuleFor(x => ShortKey).NotEmpty().MaximumLength(50);
    }
}

public record DeleteShortUrlCommand(long Id) : Command("删除短链接命令");