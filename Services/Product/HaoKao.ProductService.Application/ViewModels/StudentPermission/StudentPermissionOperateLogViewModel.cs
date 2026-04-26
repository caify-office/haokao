using HaoKao.ProductService.Domain.Entities;
using HaoKao.ProductService.Domain.Enums;
using HaoKao.ProductService.Domain.Queries;

namespace HaoKao.ProductService.Application.ViewModels.StudentPermission;

[AutoMapTo(typeof(StudentPermissionOperateLogQuery))]
[AutoMapFrom(typeof(StudentPermissionOperateLogQuery))]
public class QueryStudentPermissionOperateLogViewModel : QueryDtoBase<BrowseStudentPermissionOperateLogViewModel>
{
    /// <summary>
    /// 昵称/手机号
    /// </summary>
    public string StudentName { get; init; }

    /// <summary>
    /// 开始操作时间
    /// </summary>
    public DateTime? StartOperateTime { get; init; }

    /// <summary>
    /// 结束操作时间
    /// </summary>
    public DateTime? EndOperateTime { get; init; }
}


[AutoMapTo(typeof(StudentPermissionOperateLog))]
[AutoMapFrom(typeof(StudentPermissionOperateLog))]
public record BrowseStudentPermissionOperateLogViewModel : IDto
{
    /// <summary>
    /// 主键Id
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// 学员权限Id
    /// </summary>
    public Guid StudentPermissionId { get; init; }

    /// <summary>
    /// 学员昵称(即用户昵称)
    /// </summary>
    public string StudentName { get; init; }

    /// <summary>
    /// 学员ID（即用户ID）
    /// </summary>
    public Guid StudentId { get; init; }

    /// <summary>
    /// 产品Id
    /// </summary>
    public Guid ProductId { get; init; }

    /// <summary>
    /// 产品名称
    /// </summary>
    public string ProductName { get; init; }

    /// <summary>
    /// 产品类别
    /// </summary>
    public ProductType ProductType { get; init; }

    /// <summary>
    /// 原到期时间
    /// </summary>
    public DateTime? OldExpiredTime { get; init; }

    /// <summary>
    /// 现到期时间
    /// </summary>
    public DateTime NewExpiredTime { get; init; }

    /// <summary>
    /// 创建者Id
    /// </summary>
    public Guid CreatorId { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }
}