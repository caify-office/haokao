namespace HaoKao.BasicService.Domain.Commands.TenantAccess;

/// <summary>
/// 添加系统租户访问设置
/// </summary>
/// <param name="AccessName">当前设置的访问名称</param>
/// <param name="IsDefault">是否为默认设置</param>
/// <param name="WebSiteName">网站名称</param>
/// <param name="Introduction">站点简介</param>
/// <param name="Favicon">图站图标</param>
/// <param name="Logo">Logo 图标地址</param>
/// <param name="HttpAddress">Http访问地址</param>
/// <param name="OrganizationalUnit">组织单位</param>
/// <param name="IcpFiling">Icp 备案号</param>
/// <param name="FilingAddress">Icp备案地址链接地址</param>
/// <param name="Copyright">版权所有</param>
/// <param name="CopyrightAddress">版权链接地址</param>
/// <param name="BackgroundTheme"></param>
/// <param name="AccessCount"></param>
/// <param name="OpenRegister"></param>
public record CreateTenantAccessCommand(
    string AccessName,
    bool IsDefault,
    string WebSiteName,
    string Introduction,
    string Favicon,
    string Logo,
    string HttpAddress,
    string OrganizationalUnit,
    string IcpFiling,
    string FilingAddress,
    string Copyright,
    string CopyrightAddress,
    string BackgroundTheme,
    string AccessCount,
    bool OpenRegister
) : TenantAccessCommand(
    AccessName,
    IsDefault,
    WebSiteName,
    Introduction,
    Favicon,
    Logo,
    HttpAddress,
    OrganizationalUnit,
    IcpFiling,
    FilingAddress,
    Copyright,
    CopyrightAddress,
    AccessCount,
    OpenRegister,
    "添加系统租户访问设置"
);