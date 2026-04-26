using Girvs.BusinessBasis.Queries;
using Girvs.Extensions.Collections;
using System.Linq;

namespace HaoKao.OpenPlatformService.Infrastructure.Repositories;

public class AccessClientRepository : Repository<AccessClient>,IAccessClientRepository
{
    public async Task<AccessClient> GetByClientId(string clientId)
    {
        var baseQuery = Queryable
            .Where(x => x.ClientId == clientId);

        var client = (await baseQuery.ToArrayAsync())
            .SingleOrDefault(x => x.ClientId == clientId);
        if (client == null) return null;


        await baseQuery.Include(x => x.ClientSecrets)
              .SelectMany(c => c.ClientSecrets).LoadAsync();
        await baseQuery.Include(x => x.RedirectUris)
              .SelectMany(c => c.RedirectUris).LoadAsync();
        await baseQuery.Include(x => x.AllowedGrantTypes)
              .SelectMany(c => c.AllowedGrantTypes).LoadAsync();
        await baseQuery.Include(x => x.PostLogoutRedirectUris)
              .SelectMany(c => c.PostLogoutRedirectUris).LoadAsync();
        await baseQuery.Include(x => x.AllowedScopes)
             .SelectMany(c => c.AllowedScopes).LoadAsync();
        await baseQuery.Include(x => x.AllowedIdentityTokenSigningAlgorithms)
              .SelectMany(c => c.AllowedIdentityTokenSigningAlgorithms).LoadAsync();
        await baseQuery.Include(x => x.IdentityProviderRestrictions)
              .SelectMany(c => c.IdentityProviderRestrictions).LoadAsync();
        await baseQuery.Include(x => x.Claims)
            .SelectMany(c => c.Claims).LoadAsync();

        await baseQuery.Include(x => x.AllowedCorsOrigins)
            .SelectMany(c => c.AllowedCorsOrigins).LoadAsync();
        await baseQuery.Include(x => x.Properties)
            .SelectMany(c => c.Properties).LoadAsync();
        await baseQuery.Include(x => x.DomainProxies)
           .SelectMany(c => c.DomainProxies).LoadAsync();

        return client;
    }

    public async Task<AccessClient> GetById(Guid id)
    {
        var baseQuery = Queryable
            .Where(x => x.Id == id);

        var client = (await baseQuery.ToArrayAsync())
            .SingleOrDefault(x => x.Id == id);
        if (client == null) return null;

        await baseQuery.Include(x => x.ClientSecrets)
              .SelectMany(c => c.ClientSecrets).LoadAsync();
        await baseQuery.Include(x => x.RedirectUris)
              .SelectMany(c => c.RedirectUris).LoadAsync();
        await baseQuery.Include(x => x.AllowedGrantTypes)
              .SelectMany(c => c.AllowedGrantTypes).LoadAsync();
        await baseQuery.Include(x => x.PostLogoutRedirectUris)
              .SelectMany(c => c.PostLogoutRedirectUris).LoadAsync();
        await baseQuery.Include(x => x.AllowedScopes)
             .SelectMany(c => c.AllowedScopes).LoadAsync();
        await baseQuery.Include(x => x.AllowedIdentityTokenSigningAlgorithms)
              .SelectMany(c => c.AllowedIdentityTokenSigningAlgorithms).LoadAsync();
        await baseQuery.Include(x => x.IdentityProviderRestrictions)
              .SelectMany(c => c.IdentityProviderRestrictions).LoadAsync();
        await baseQuery.Include(x => x.Claims)
            .SelectMany(c => c.Claims).LoadAsync();

        await baseQuery.Include(x => x.AllowedCorsOrigins)
            .SelectMany(c => c.AllowedCorsOrigins).LoadAsync();
        await baseQuery.Include(x => x.Properties)
            .SelectMany(c => c.Properties).LoadAsync();
        await baseQuery.Include(x => x.DomainProxies)
           .SelectMany(c => c.DomainProxies).LoadAsync();

        return client;
    }

    public override async Task<List<AccessClient>> GetByQueryAsync(QueryBase<AccessClient> query)
    {
        query.RecordCount = await Queryable.Where(query.GetQueryWhere()).CountAsync();

        if (query.RecordCount < 1)
        {
            query.Result = [];
        }
        else
        {
            query.Result =
                await Queryable.Where(query.GetQueryWhere())
                               .SelectProperties(query.QueryFields)
                               .OrderByDescending(x => x.CreateTime)
                               .Skip(query.PageStart)
                               .Take(query.PageSize).ToListAsync();
        }

        return query.Result;
    }
}