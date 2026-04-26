namespace HaoKao.ProductService.Domain.Commands.SupervisorStudent;

/// <summary>
/// 删除督学学员命令
/// </summary>
/// <param name="Id">主键</param>
public record DeleteSupervisorStudentCommand(Guid Id) : Command("删除督学学员");