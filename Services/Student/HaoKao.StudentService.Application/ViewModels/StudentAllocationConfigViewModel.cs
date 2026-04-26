using HaoKao.StudentService.Domain.Commands;
using HaoKao.StudentService.Domain.Entities;

namespace HaoKao.StudentService.Application.ViewModels;

[AutoMapFrom(typeof(StudentAllocationConfig))]
public record BrowseStudentAllocationConfigViewModel : IDto
{
    /// <summary>
    /// 配置数据
    /// </summary>
    public HashSet<PercentageAllocation> Data { get; init; }

    /// <summary>
    /// 分配方式
    /// </summary>
    public WaysOfAllocation WaysOfAllocation { get; init; }
}

[AutoMapTo(typeof(SaveStudentAllocationConfigCommand))]
public record SaveStudentAllocationConfigViewModel : IDto
{
    /// <summary>
    /// 配置数据
    /// </summary>
    public HashSet<PercentageAllocation> Data { get; init; }

    /// <summary>
    /// 分配方式
    /// </summary>
    public WaysOfAllocation WaysOfAllocation { get; init; }

    /// <summary>
    /// 租户Id
    /// </summary>
    public Guid TenantId { get; init; }
}