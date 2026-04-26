using HaoKao.BasicService.Domain.Enumerations;

namespace HaoKao.BasicService.Domain.Commands.User;

/// <summary>
/// 通过事件创建用户
/// </summary>
/// <param name="UserAccount">用户账户</param>
/// <param name="UserPassword">用户密码</param>
/// <param name="UserName">用户名称</param>
/// <param name="ContactNumber">用户联系方式</param>
/// <param name="State">用户的状态</param>
/// <param name="UserType">用户类型</param>
/// <param name="OtherId">绑定其它相关服务的关键标识Id</param>
/// <param name="TenantId">租户Id</param>
/// <param name="TenantName">租户名称</param>
public record EventCreateUserCommand(
    string UserAccount,
    string UserPassword,
    string UserName,
    string ContactNumber,
    DataState State,
    UserType UserType,
    Guid OtherId,
    Guid TenantId,
    string TenantName
) : UserCommand(
    UserAccount,
    UserName,
    ContactNumber,
    UserPassword,
    "通过事件创建用户"
)
{
    public EventCreateUserCommand(
        string userAccount,
        string userPassword,
        string userName,
        string contactNumber,
        DataState state,
        UserType userType,
        Guid tenantId,
        string tenantName
    ) : this(
        userAccount,
        userPassword,
        userName,
        contactNumber,
        state,
        userType,
        Guid.Empty,
        tenantId,
        tenantName
    ) { }
}