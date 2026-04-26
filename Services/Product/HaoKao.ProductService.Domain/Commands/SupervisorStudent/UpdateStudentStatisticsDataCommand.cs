namespace HaoKao.ProductService.Domain.Commands.SupervisorStudent;

/// <summary>
/// 更新督学学员统计数据
/// </summary>
public record UpdateStudentStatisticsDataCommand(Guid SupervisorClassId, Guid ProductId) : Command("更新督学学员统计数据");