using IdentityServer4.Models;

namespace HaoKao.OpenPlatformService.Application.ViewModels.Account;

public class ErrorViewModel
{
    public ErrorViewModel()
    {
    }

    public ErrorViewModel(string error)
    {
        Error = new ErrorMessage { Error = error };
    }

    public ErrorMessage Error { get; set; }
}