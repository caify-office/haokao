namespace HaoKao.ProductService.Domain.Commands.SupervisorClass;

/// <summary>
/// 删除班级督学命令
/// </summary>
/// <param name="Id">主键</param>
public record DeleteSupervisorClassCommand(Guid Id) : Command("删除班级督学");