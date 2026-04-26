using Girvs.Configuration;

namespace HaoKao.CouponService.Application.Configuration;

public class CommonParamterConfig : IAppModuleConfig
{

    public string AppKey { get; set; } = "wx1917f27b2522e1ce";

    public string APPSecret { get; set; } = "4fd38646436d0a5110f76ffedfbf90dd";

    public string GrantType { get; set; } = "client_credential";

    public string GetCodeUrl { get; set; } = "https://api.weixin.qq.com/cgi-bin/wxaapp/createwxaqrcode";

    public string GetWeiXinTokenUrl { get; set; } = "https://api.weixin.qq.com/cgi-bin/token";
    
   
    public void Init() { }

}