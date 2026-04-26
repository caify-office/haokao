using Girvs.Extensions;
using HaoKao.OpenPlatformService.Application.IdentityServerExtensions.Extension;
using HaoKao.OpenPlatformService.Domain.Repositories;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.Extensions.Logging;

namespace HaoKao.OpenPlatformService.Application.IdentityServerExtensions.Service;

public class HaoKaoProfileService(
    IRegisterUserRepository repository,
    ILogger<HaoKaoProfileService> logger
) : IProfileService
{
    private readonly IRegisterUserRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly ILogger<HaoKaoProfileService> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        context.LogProfileRequest(_logger);

        if (context.RequestedClaimTypes.Any())
        {
            var userId = context.Subject.GetSubjectId();
            var user = await _repository.GetByIdAsync(userId.ToGuid()) ?? throw new GirvsException("用户不存在");
            context.AddRequestedClaims(user.BuildClaims());
        }

        context.LogIssuedClaims(_logger);
    }

    public async Task IsActiveAsync(IsActiveContext context)
    {
        _logger.LogDebug("IsActive called from: {caller}", context.Caller);
        var userId = context.Subject.GetSubjectId().ToGuid();
        var user = await _repository.GetByIdAsync(userId);
        context.IsActive = user != null && user.UserState == UserState.Enable;
    }
}