using HaoKao.BasicService.Domain.Entities;

namespace HaoKao.BasicService.Domain.Commands.User;

/// <summary>
/// 更新用户的数据权限
/// </summary>
/// <param name="UserId">用户Id</param>
/// <param name="UserRules">用户权限集合</param>
public record UpdateUserRuleCommand(Guid UserId, List<UserRule> UserRules) : Command("更新用户的数据权限");