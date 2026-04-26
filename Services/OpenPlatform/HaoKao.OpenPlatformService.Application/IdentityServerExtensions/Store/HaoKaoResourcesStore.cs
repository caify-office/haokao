using IdentityServer4.Models;
using IdentityServer4.Stores;

namespace HaoKao.OpenPlatformService.Application.IdentityServerExtensions.Store;

public class HaoKaoResourcesStore : IResourceStore
{
    public const string ApiServiceResourceGroupName = "apiservice";

    private static Task<List<ApiScope>> GetApiScopes()
    {
        return Task.FromResult(new List<ApiScope> { new(ApiServiceResourceGroupName) });
    }

    private static Task<IdentityResource[]> GetIdentityResource()
    {
        return Task.FromResult(
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Phone(),
                new IdentityResources.Email()
            }
        );
    }

    private static Task<IEnumerable<ApiResource>> GetApiResource()
    {
        var services = new[]
        {
            "haokao-studentservice-webapi",
            "haokao-useranswerrecordservice-webapi",
            "haokao-userquestioncollectionservice-webapi",
            "haokao-openplatformservice-webapi",
            "haokao-tenantservice-webapi",
            "haokao-userwronganswerservice-webapi",
            "haokao-questionservice-webapi",
            "haokao-notificationmessageservice-webapi",
            "haokao-paperservice-webapi",
            "haokao-auditlogservice-webapi",
            "haokao-questioncategoryservice-webapi",
            "haokao-groupbookingservice-webapi",
            "haokao-datadictionaryservice-webapi",
            "haokao-basicservice-webapi",
            "haokao-chapternodeservice-webapi",
            "haokao-knowledgepointservice-webapi",
            "haokao-papertempleteservice-webapi",
            "haokao-subjectservice-webapi",
            "haokao-errorcorrectingservice-webapi",
            "haokao-questionbankmemberservice-webapi",
            "haokao-orderservice-webapi",
            "haokao-answeringquestionservice-webapi",
            "haokao-learnprogressservice-webapi",
            "haokao-productservice-webapi",
            "haokao-courseservice-webapi",
            "haokao-courseratingservice-webapi",
            "haokao-coursefeatureservice-webapi",
            "haokao-agreementservice-webapi",
            "haokao-studymaterialservice-webapi",
            "haokao-continuationservice-webapi",
            "haokao-feedbackservice-webapi",
            "haokao-couponservice-webapi",
            "haokao-noticeservice-webapi",
            "haokao-livebroadcastservice-webapi",
            "haokao-campaignservice-webapi",
            "haokao-drawprizeservice-webapi",
            "haokao-correctionnotebook-webapi",
            "haokao-burialpointservice-webapi",
            "haokao-learningplanservice-webapi",
            "haokao-deepseekservice-webapi",
        };


        //var consulConfig = EngineContext.Current.GetAppModuleConfig<ConsulConfig>();

        //var client = new ConsulClient(x => x.Address = new Uri(consulConfig.ConsulAddress));
        //var queryResult = await client.Agent.Services();
        //var services = queryResult.Response.Select(x => x.Value);

        return Task.FromResult(services.Select(x => new ApiResource(x)
        {
            ApiSecrets = { new Secret(x.Sha256()) },
            Scopes = { ApiServiceResourceGroupName }
        }));
    }

    public async Task<Resources> GetAllResourcesAsync()
    {
        var identityResource = await GetIdentityResource();
        var apiResource = await GetApiResource();
        // var apiScope =  await GetApiScopes();
        return new Resources(identityResource, apiResource, null);
    }

    public async Task<IEnumerable<ApiResource>> FindApiResourcesByNameAsync(
        IEnumerable<string> apiResourceNames)
    {
        if (apiResourceNames == null) throw new ArgumentNullException(nameof(apiResourceNames));

        var apiResource = await GetApiResource();

        return apiResource.Where(a => apiResourceNames.Contains(a.Name));
    }

    /// <inheritdoc />
    public async Task<IEnumerable<IdentityResource>> FindIdentityResourcesByScopeNameAsync(
        IEnumerable<string> scopeNames)
    {
        if (scopeNames == null) throw new ArgumentNullException(nameof(scopeNames));

        var identityResource = await GetIdentityResource();

        return identityResource.Where(i => scopeNames.Contains(i.Name));
    }

    /// <inheritdoc />
    public async Task<IEnumerable<ApiResource>> FindApiResourcesByScopeNameAsync(
        IEnumerable<string> scopeNames)
    {
        if (scopeNames == null) throw new ArgumentNullException(nameof(scopeNames));

        var apiResource = await GetApiResource();

        return apiResource.Where(a => a.Scopes.Any(scopeNames.Contains));
    }

    /// <inheritdoc />
    public async Task<IEnumerable<ApiScope>> FindApiScopesByNameAsync(IEnumerable<string> scopeNames)
    {
        if (scopeNames == null) throw new ArgumentNullException(nameof(scopeNames));

        var scopes = await GetApiScopes();

        return scopes.Where(x => scopeNames.Contains(x.Name));
    }
}