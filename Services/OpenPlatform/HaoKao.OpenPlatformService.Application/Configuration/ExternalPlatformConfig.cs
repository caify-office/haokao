using Girvs.Configuration;

namespace HaoKao.OpenPlatformService.Application.Configuration;

public class ExternalPlatformConfig : IAppModuleConfig
{
    public string ExternalAuthenticationProxyUrl { get; set; } = "http://192.168.51.166";

    public List<WeixinApp> WeixinApps { get; set; } = [];

    public Dictionary<string, string> WeixinApis { get; set; } = [];

    public string ClientDomain { get; set; } = "https://accounts.haokao123.com";

    public string ClientLoginUrl { get; set; } = "https://accounts.haokao123.com/account/login";

    public string ClientLogoutUrl { get; set; } = "https://accounts.haokao123.com/account/login";

    public string ClientErrorUrl { get; set; } = "https://accounts.haokao123.com/account/error";

    public string ClientConsentUrl { get; set; } = "https://accounts.haokao123.com/account/consent";

    public string ClientDeviceVerificationUrl { get; set; } = "https://accounts.haokao123.com/account/device";

    public HaoKaoAuthenticationOptions WeixinAuthenticationOptions { get; set; } = new();

    public HaoKaoAuthenticationOptions GiteeAuthenticationOptions { get; set; } = new();

    public HaoKaoAuthenticationOptions GitHubAuthenticationOptions { get; set; } = new();

    public HaoKaoAppleAuthenticationOptions AppleAuthenticationOptions { get; set; } = new();

    public void Init() { }

    public class HaoKaoAuthenticationOptions
    {
        public bool Enable { get; set; } = true;

        public string ClientId { get; set; } = string.Empty;

        public string ClientSecret { get; set; } = string.Empty;

        public string CallbackPathPrefix { get; set; } = "/api";

        public CookieSecurePolicy CookieSecurePolicy { get; set; } = CookieSecurePolicy.Always;
    }

    public class HaoKaoAppleAuthenticationOptions : HaoKaoAuthenticationOptions
    {
        public string TeamId { get; set; } = "";

        public string KeyId { get; set; } = "QV46CV33M7";

        public string PrviateKeyFilePath { get; set; } = "App_Data/AuthKey_LKAUA32YYK.p8";
    }

    public class WeixinApp
    {
        public Guid Id { get; init; }

        public string AppName { get; init; }

        public string AppId { get; init; }

        public string AppSecret { get; init; }
    }
}