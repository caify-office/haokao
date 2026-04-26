using HaoKao.BasicService.Domain.Enumerations;

namespace HaoKao.BasicService.Domain.Commands.User;

/// <summary>
/// 更新用户
/// </summary>
/// <param name="Id">用户Id</param>
/// <param name="UserPassword">用户密码</param>
/// <param name="UserName">用户名称</param>
/// <param name="ContactNumber">用户联系方式</param>
/// <param name="State">用户的状态</param>
/// <param name="UserType">用户类型</param>
public record UpdateUserCommand(
    Guid Id,
    string UserPassword,
    string UserName,
    string ContactNumber,
    DataState State,
    UserType UserType
) : UserCommand(
    string.Empty,
    UserName,
    ContactNumber,
    UserPassword,
    "更新用户"
);