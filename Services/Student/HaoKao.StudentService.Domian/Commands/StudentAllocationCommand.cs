namespace HaoKao.StudentService.Domain.Commands;

/// <summary>
/// 批量修改分配命令
/// </summary>
/// <param name="Ids"></param>
/// <param name="SalespersonId"></param>
/// <param name="SalespersonName"></param>
public record UpdateAllocateToCommand(IReadOnlyList<Guid> Ids, Guid SalespersonId, string SalespersonName) : Command("批量修改分配命令");

/// <summary>
/// 修改备注命令
/// </summary>
/// <param name="Id"></param>
/// <param name="Remark"></param>
public record UpdateRemarkCommand(Guid Id, string Remark) : Command("修改备注命令");