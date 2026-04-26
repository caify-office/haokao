namespace HaoKao.CouponService.Application.ViewModels.Refit;

public record WeiXinAccessTokenBody
{
    public string appid { get; init; }

    public string secret { get; init; }

    public string grant_type => "client_credential";

    public bool force_refresh { get; init; }
}

public class WeiXinAccessTokenViewModel
{
    public string Access_token { get; set; }

    public int Expires_in { get; set; }
}