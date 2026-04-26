namespace HaoKao.BasicService.Domain.Commands.TenantAccess;

/// <summary>
/// 设置为默认的系统租户访问设置
/// </summary>
/// <param name="Id">系统租户访问设置Id</param>
public record SetDefaultTenantAccessCommand(Guid Id) : Command("设置为默认的系统租户访问设置");