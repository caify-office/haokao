namespace HaoKao.BasicService.Domain.Commands.User;

/// <summary>
/// 通过事件修改用户
/// </summary>
/// <param name="TenantId">租户Id</param>
/// <param name="TenantName">租户名称</param>
/// <param name="UserAccount">用户账户</param>
/// <param name="UserPassword">用户密码</param>
/// <param name="UserName">用户名称</param>
/// <param name="ContactNumber">用户联系方式</param>
public record EventEditUserCommand(
    Guid TenantId,
    string TenantName,
    string UserAccount,
    string UserPassword,
    string UserName,
    string ContactNumber
) : Command("通过事件修改用户");