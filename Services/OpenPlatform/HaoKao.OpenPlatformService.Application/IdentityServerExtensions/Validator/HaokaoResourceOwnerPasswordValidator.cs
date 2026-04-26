using HaoKao.OpenPlatformService.Application.IdentityServerExtensions.Extension;
using HaoKao.OpenPlatformService.Domain.Repositories;
using IdentityModel;
using IdentityServer4.Events;
using IdentityServer4.Services;
using IdentityServer4.Validation;

namespace HaoKao.OpenPlatformService.Application.IdentityServerExtensions.Validator;

public class HaoKaoResourceOwnerPasswordValidator(IEventService events, IRegisterUserRepository repository) : IResourceOwnerPasswordValidator
{
    private readonly IEventService _events = events ?? throw new ArgumentNullException(nameof(events));
    private readonly IRegisterUserRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));

    public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
    {
        var user = await _repository.GetAsync(x => x.Phone == context.UserName);

        await _events.RaiseAsync(new UserLoginSuccessEvent(context.UserName, user.Id.ToString(), context.UserName, interactive: false));

        var result = new GrantValidationResult(user.Id.ToString(), OidcConstants.AuthenticationMethods.Password, user.BuildClaims());

        context.Result = result;
    }
}