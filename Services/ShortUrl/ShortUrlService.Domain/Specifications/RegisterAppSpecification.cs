using ShortUrlService.Domain.Entities;
using ShortUrlService.Domain.Specifications.Base;

namespace ShortUrlService.Domain.Specifications;

public sealed class RegisterAppByNameAndCodeSpec : BaseSpecification<RegisterApp>
{
    public RegisterAppByNameAndCodeSpec(string appName, string appCode)
    {
        AddCriteria(x => x.AppName == appName && x.AppCode == appCode);
    }
}

public sealed class RegisterAppByCodeAndSecretSpec : BaseSpecification<RegisterApp>
{
    public RegisterAppByCodeAndSecretSpec(string appCode, string appSecret)
    {
        AddCriteria(x => x.AppCode == appCode && x.AppSecret == appSecret);
    }
}