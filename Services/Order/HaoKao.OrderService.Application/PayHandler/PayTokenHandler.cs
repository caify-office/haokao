using JsonSerializer = System.Text.Json.JsonSerializer;

namespace HaoKao.OrderService.Application.PayHandler;

public static class PayTokenHandler
{
    private const string _UserId = "u";
    private const string _UserName = "un";
    private const string _TenantId = "t";
    private const string _TenantName = "tn";

    public static string GetAttachment()
    {
        var otherClaims = EngineContext.Current.ClaimManager.IdentityClaim.OtherClaims;
        var newClaims = new Dictionary<string, string>();
        if (otherClaims.TryGetValue(GirvsIdentityClaimTypes.UserId, out var userId))
        {
            newClaims.Add(_UserId, userId);
        }
        // if (otherClaims.TryGetValue(GirvsIdentityClaimTypes.UserName, out var userName))
        // {
        //     newClaims.Add(_UserName, userName);
        // }
        if (otherClaims.TryGetValue(GirvsIdentityClaimTypes.TenantId, out var tenantId))
        {
            newClaims.Add(_TenantId, tenantId);
        }
        // if (otherClaims.TryGetValue(GirvsIdentityClaimTypes.TenantName, out var tenantName))
        // {
        //     newClaims.Add(_TenantName, tenantName);
        // }
        return JsonSerializer.Serialize(newClaims);
    }

    public static Dictionary<string, string> ParseAttachment(string attachment)
    {
        var dic = JsonSerializer.Deserialize<Dictionary<string, string>>(attachment);
        var newClaims = new Dictionary<string, string>();
        if (dic.TryGetValue(_UserId, out var userId))
        {
            newClaims.Add(GirvsIdentityClaimTypes.UserId, userId);
        }
        if (dic.TryGetValue(_UserName, out var userName))
        {
            newClaims.Add(GirvsIdentityClaimTypes.UserName, userName);
        }
        if (dic.TryGetValue(_TenantId, out var tenantId))
        {
            newClaims.Add(GirvsIdentityClaimTypes.TenantId, tenantId);
        }
        if (dic.TryGetValue(_TenantName, out var tenantName))
        {
            newClaims.Add(GirvsIdentityClaimTypes.TenantName, tenantName);
        }
        return newClaims;
    }
}