namespace HaoKao.StudentService.Domain.Commands;

/// <summary>
/// 创建学员命令
/// </summary>
/// <param name="RegisterUserId"></param>
public record CreateStudentCommand(Guid RegisterUserId) : Command("创建学员命令");

/// <summary>
/// 更新学员命令
/// </summary>
/// <param name="RegisterUserId"></param>
/// <param name="IsPaidStudent"></param>
public record UpdateStudentCommand(Guid RegisterUserId, bool IsPaidStudent) : Command("更新学员命令");