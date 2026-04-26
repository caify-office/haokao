using ShortUrlService.Domain.Entities;
using ShortUrlService.Domain.Repositories;
using ShortUrlService.Domain.Specifications;
using ShortUrlService.Infrastructure.Extensions;
using ShortUrlService.Infrastructure.Repositories.Base;

namespace ShortUrlService.Infrastructure.Repositories;

public class RegisterAppRepository(ShortUrlDbContext dbContext) : Repository<RegisterApp, long>(dbContext), IRegisterAppRepository
{
    public Task<bool> ExistForCreate(string appName, string appCode)
    {
        var specification = new RegisterAppByNameAndCodeSpec(appName, appCode);
        return AnyAsync(specification);
    }

    public Task<RegisterApp?> GetByCodeAndSecret(string appCode, string appSecret)
    {
        var specification = new RegisterAppByCodeAndSecretSpec(appCode, appSecret);
        return dbContext.RegisterApps.SatisfiedBy(specification).Select(x => new RegisterApp
        {
            Id = x.Id,
            AppDomains = x.AppDomains,
        }).FirstOrDefaultAsync();
    }
}