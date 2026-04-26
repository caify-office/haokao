namespace ShortUrlService.WebApi.Configurations;

public class IpRateLimitConfig : IpRateLimitOptions, IAppModuleConfig
{
    public void Init() { }
}

public class IpRateLimitPoliciesConfig : IpRateLimitPolicies, IAppModuleConfig
{
    public void Init() { }
}