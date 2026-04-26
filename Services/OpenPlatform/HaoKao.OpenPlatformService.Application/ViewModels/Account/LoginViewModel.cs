using HaoKao.OpenPlatformService.Application.ViewModels.Identity;

namespace HaoKao.OpenPlatformService.Application.ViewModels.Account;

public class LoginViewModel
{
    public bool AllowRememberLogin { get; set; } = true;

    public bool EnableLocalLogin { get; set; } = true;

    public IEnumerable<ExternalProvider> ExternalProviders { get; set; } = [];

    public IEnumerable<ExternalProvider> VisibleExternalProviders => ExternalProviders.Where(x => !string.IsNullOrWhiteSpace(x.DisplayName));

    public bool IsExternalLoginOnly => EnableLocalLogin == false && ExternalProviders?.Count() == 1;

    public string ExternalLoginScheme => IsExternalLoginOnly ? ExternalProviders?.SingleOrDefault()?.AuthenticationScheme : null;

    public bool RememberLogin { get; set; }

    public string ReturnUrl { get; set; }

    public string Username { get; set; }
}