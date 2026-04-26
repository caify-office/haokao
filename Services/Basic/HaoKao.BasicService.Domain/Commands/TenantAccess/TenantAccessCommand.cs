namespace HaoKao.BasicService.Domain.Commands.TenantAccess;

/// <summary>
/// ϵͳ�⻧��������
/// </summary>
/// <param name="AccessName">��ǰ���õķ�������</param>
/// <param name="IsDefault">�Ƿ�ΪĬ������</param>
/// <param name="WebSiteName">��վ����</param>
/// <param name="Introduction">վ����</param>
/// <param name="Favicon">ͼվͼ��</param>
/// <param name="Logo">Logo ͼ���ַ</param>
/// <param name="HttpAddress">Http���ʵ�ַ</param>
/// <param name="OrganizationalUnit">��֯��λ</param>
/// <param name="IcpFiling">Icp ������</param>
/// <param name="FilingAddress">Icp������ַ���ӵ�ַ</param>
/// <param name="Copyright">��Ȩ����</param>
/// <param name="CopyrightAddress">��Ȩ���ӵ�ַ</param>
/// <param name="AccessCount"></param>
/// <param name="OpenRegister"></param>
/// <param name="CommandDesc">��������</param>
public abstract record TenantAccessCommand(
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
    string AccessCount,
    bool OpenRegister,
    string CommandDesc
) : Command(CommandDesc);