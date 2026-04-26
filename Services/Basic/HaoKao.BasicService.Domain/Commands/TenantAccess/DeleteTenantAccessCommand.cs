namespace HaoKao.BasicService.Domain.Commands.TenantAccess;

/// <summary>
/// 删除系统租户访问设置
/// </summary>
/// <param name="Id">租户访问设置Id</param>
public record DeleteTenantAccessCommand(Guid Id) : Command("删除系统租户访问设置");