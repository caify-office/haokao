using HaoKao.BasicService.Domain.Entities;
using HaoKao.BasicService.Domain.Repositories;

namespace HaoKao.BasicService.Domain.Extensions;

public static class EngineContextExtensions
{
    public static User GetCurrentUser(this IEngine engine)
    {
        var userId = engine.ClaimManager.IdentityClaim.UserId;
        if (userId.IsNullOrEmpty()) return null;
        var userRepository = engine.Resolve<IUserRepository>();
        return userRepository.GetByIdAsync(userId.ToHasGuid().Value).Result;
    }
}