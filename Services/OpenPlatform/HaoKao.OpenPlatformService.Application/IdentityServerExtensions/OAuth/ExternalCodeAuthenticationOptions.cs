using Newtonsoft.Json;

namespace HaoKao.OpenPlatformService.Application.IdentityServerExtensions.OAuth;

public class ExternalCodeAuthenticationOptions
{
    /// <summary>
    /// 名称
    /// </summary>
    [Required]
    public string Scheme { get; set; }

    /// <summary>
    /// Mini Program、App或者 其它
    /// </summary>
    [Required]
    public string SchemeType { get; set; }

    /// <summary>
    /// 临时登录凭证code
    /// </summary>
    [Required]
    public string Code { get; set; }

    /// <summary>
    /// Gets or sets the provider-assigned client id.
    /// </summary>
    [Required]
    public string ClientId { get; set; } = default!;

    /// <summary>
    /// Gets or sets the provider-assigned client secret.
    /// </summary>
    [System.Text.Json.Serialization.JsonIgnore, JsonIgnore]
    public string ClientSecret { get; set; } = default!;
}