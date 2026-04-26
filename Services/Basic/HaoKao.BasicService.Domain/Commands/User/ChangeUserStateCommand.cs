using HaoKao.BasicService.Domain.Enumerations;

namespace HaoKao.BasicService.Domain.Commands.User;

/// <summary>
/// 更新用户状态
/// </summary>
/// <param name="Id">用户Id</param>
/// <param name="State">用户状态</param>
public record ChangeUserStateCommand(Guid Id, DataState State) : Command("更新用户状态");