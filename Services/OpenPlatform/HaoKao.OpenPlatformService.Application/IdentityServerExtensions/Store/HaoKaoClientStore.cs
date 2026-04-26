using HaoKao.OpenPlatformService.Domain.Entities;
using HaoKao.OpenPlatformService.Domain.Repositories;
using IdentityServer4.Models;
using IdentityServer4.Stores;

namespace HaoKao.OpenPlatformService.Application.IdentityServerExtensions.Store;

public class HaoKaoClientStore(IAccessClientRepository repository) : IClientStore
{
    private readonly IAccessClientRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));

    public async Task<Client> FindClientByIdAsync(string clientId)
    {
        var accessClient = await _repository.GetByClientId(clientId);
        return accessClient != null ? ConvertClient(accessClient) : null;
    }

    private static Client ConvertClient(AccessClient accessClient)
    {
        var client = new Client
        {
            Enabled = accessClient.Enabled,
            ClientId = accessClient.ClientId,
            ProtocolType = accessClient.ProtocolType,
            RequireClientSecret = accessClient.RequireClientSecret,
            ClientName = accessClient.ClientName,
            Description = accessClient.Description,
            ClientUri = accessClient.ClientUri,
            LogoUri = accessClient.LogoUri,
            RequireConsent = accessClient.RequireConsent,
            AllowRememberConsent = accessClient.AllowRememberConsent,
            RequirePkce = accessClient.RequirePkce,
            AllowPlainTextPkce = accessClient.AllowPlainTextPkce,
            RequireRequestObject = accessClient.RequireRequestObject,
            AllowAccessTokensViaBrowser = accessClient.AllowAccessTokensViaBrowser,
            FrontChannelLogoutUri = accessClient.FrontChannelLogoutUri,
            FrontChannelLogoutSessionRequired = accessClient.FrontChannelLogoutSessionRequired,
            BackChannelLogoutUri = accessClient.BackChannelLogoutUri,
            BackChannelLogoutSessionRequired = accessClient.BackChannelLogoutSessionRequired,
            AllowOfflineAccess = accessClient.AllowOfflineAccess,
            AlwaysIncludeUserClaimsInIdToken = accessClient.AlwaysIncludeUserClaimsInIdToken,
            IdentityTokenLifetime = accessClient.IdentityTokenLifetime,
            AccessTokenLifetime = accessClient.AccessTokenLifetime,
            AuthorizationCodeLifetime = accessClient.AuthorizationCodeLifetime,
            AbsoluteRefreshTokenLifetime = accessClient.AbsoluteRefreshTokenLifetime,
            SlidingRefreshTokenLifetime = accessClient.SlidingRefreshTokenLifetime,
            ConsentLifetime = accessClient.ConsentLifetime,
            RefreshTokenUsage = (TokenUsage)accessClient.RefreshTokenUsage,
            UpdateAccessTokenClaimsOnRefresh = accessClient.UpdateAccessTokenClaimsOnRefresh,
            RefreshTokenExpiration = (TokenExpiration)accessClient.RefreshTokenExpiration,
            AccessTokenType = (AccessTokenType)accessClient.AccessTokenType,
            EnableLocalLogin = accessClient.EnableLocalLogin,
            IncludeJwtId = accessClient.IncludeJwtId,
            AlwaysSendClientClaims = accessClient.AlwaysSendClientClaims,
            ClientClaimsPrefix = accessClient.ClientClaimsPrefix,
            PairWiseSubjectSalt = accessClient.PairWiseSubjectSalt,
            UserSsoLifetime = accessClient.UserSsoLifetime,
            UserCodeType = accessClient.UserCodeType,
            DeviceCodeLifetime = accessClient.DeviceCodeLifetime,
            AllowedScopes = accessClient.AllowedScopes?.Select(x => x.Scope).ToList(),
            AllowedIdentityTokenSigningAlgorithms = accessClient.AllowedIdentityTokenSigningAlgorithms?.Select(x => x.SigningAlgorithm).ToList(),
            IdentityProviderRestrictions = accessClient.IdentityProviderRestrictions?.Select(x => x.Provider).ToList(),
            AllowedGrantTypes = accessClient.AllowedGrantTypes?.Select(x => x.GrantType).ToList(),
            Claims = accessClient.Claims?.Select(x => new ClientClaim { Type = x.Type, Value = x.Value }).ToList(),
            Properties = accessClient.Properties?.ToDictionary(x => x.Key, x => x.Value),
            ClientSecrets = accessClient.ClientSecrets?.Select(p => new Secret(p.Value.Sha256())).ToList(),
            AllowedCorsOrigins = accessClient.AllowedCorsOrigins?.Select(x => x.Origin).ToList(),
        };

        if (accessClient.DomainProxies?.Count > 0)
        {
            foreach (var domainProxy in accessClient.DomainProxies)
            {
                var domain = domainProxy.Domain;
                if (string.IsNullOrWhiteSpace(domain)) continue;

                foreach (var accessClientRedirectUri in accessClient.RedirectUris)
                {
                    client.RedirectUris.Add($"{domain}{accessClientRedirectUri.RedirectUri}");
                }

                foreach (var accessClientPostLogoutRedirectUri in accessClient.PostLogoutRedirectUris)
                {
                    client.PostLogoutRedirectUris.Add($"{domain}{accessClientPostLogoutRedirectUri.PostLogoutRedirectUri}");
                }

                client.AllowedCorsOrigins.Add($"{domain}");
            }
        }
        else
        {
            client.RedirectUris = accessClient.RedirectUris?.Select(x => x.RedirectUri).ToList();
            client.PostLogoutRedirectUris = accessClient.PostLogoutRedirectUris?.Select(x => x.PostLogoutRedirectUri).ToList();
            client.AllowedCorsOrigins = accessClient.AllowedCorsOrigins?.Select(x => x.Origin).ToList();
        }

        return client;
    }
}