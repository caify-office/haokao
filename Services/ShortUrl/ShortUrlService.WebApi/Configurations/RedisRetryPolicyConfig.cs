namespace ShortUrlService.WebApi.Configurations;

public record RedisRetryPolicyConfig : IAppModuleConfig
{
    public void Init() { }

    public int[] SleepDurations { get; init; } = [];
}