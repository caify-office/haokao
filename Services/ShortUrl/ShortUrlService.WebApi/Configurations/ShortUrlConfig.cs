namespace ShortUrlService.WebApi.Configurations;

public record ShortUrlConfig : IAppModuleConfig
{
    public string Secrect { get; init; } = "s9LFkgy5RovixI1aOf8UhdY3r4DMplQZJXPqebE0WSjBn7wVzmN2Gc6THCAKut";

    public int CodeLength { get; init; } = 6;

    public Uri HostDomain { get; init; } = new("http://localhost:5000");

    public int SemaphoreCount { get; init; } = 1;

    public int LogBufferSize { get; init; } = 10;

    public bool TestAccess { get; init; } = false;

    public void Init() { }
}