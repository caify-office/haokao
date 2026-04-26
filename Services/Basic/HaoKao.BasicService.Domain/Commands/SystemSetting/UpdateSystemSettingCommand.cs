namespace HaoKao.BasicService.Domain.Commands.SystemSetting;

/// <summary>
/// 修改系统基础信息
/// </summary>
/// <param name="WebSiteName">网站名称</param>
/// <param name="Introduction">站点简介</param>
/// <param name="Logo">Logo 图标地址</param>
/// <param name="HttpAddress">Http访问地址</param>
/// <param name="OrganizationalUnit">组织单位</param>
/// <param name="IcpFiling">Icp 备案号</param>
/// <param name="FilingAddress">Icp备案地址链接地址</param>
/// <param name="Copyright">版权所有</param>
/// <param name="CopyrightAddress">版权链接地址</param>
/// <param name="WeChatImage">公众号图标地址</param>
/// <param name="WeChatAppId">公众号AppId</param>
/// <param name="MiniOAddress">MiniO存储地址配置</param>
/// <param name="MiniOAppId">MiniO AppId</param>
/// <param name="MiniOAppSecret">MiniO AppSecret</param>
/// <param name="MiniOBucket">MiniO存储桶名称</param>
/// <param name="MiniOPort">MiniO端口</param>
/// <param name="MiniOUseSSl">MiniOUseSSl</param>
/// <param name="Favicon">图标</param>
/// <param name="IsForceFollow">是否强制关注公众号</param>
/// <param name="BackgroundTheme"></param>
/// <param name="AccessCount"></param>
/// <param name="OpenRegister"></param>
public record UpdateSystemSettingCommand(
    string WebSiteName,
    string Introduction,
    string Logo,
    string HttpAddress,
    string OrganizationalUnit,
    string IcpFiling,
    string FilingAddress,
    string Copyright,
    string CopyrightAddress,
    string WeChatImage,
    string WeChatAppId,
    string MiniOAddress,
    string MiniOAppId,
    string MiniOAppSecret,
    string MiniOBucket,
    string MiniOPort,
    bool MiniOUseSSl,
    string Favicon,
    bool IsForceFollow,
    string BackgroundTheme,
    string AccessCount,
    bool OpenRegister
) : Command("修改系统基础信息");