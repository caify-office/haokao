using HaoKao.ProductService.Domain.Enums;

namespace HaoKao.ProductService.Application.ViewModels.StudentPermission;

[AutoMapFrom(typeof(Domain.Entities.StudentPermission))]
public class BrowseStudentPermissionViewModel : IDto
{
    /// <summary>
    /// 学员昵称(即用户昵称)
    /// </summary>
    public string StudentName { get; set; }

    /// <summary>
    /// 学员ID（即用户ID）
    /// </summary>
    public Guid StudentId { get; set; }

    /// <summary>
    /// 对应的订单号
    /// </summary>
    public string OrderNumber { get; set; }

    /// <summary>
    /// 来源方式
    /// </summary>
    public SourceMode SourceMode { get; set; }

    /// <summary>
    /// 产品Id
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// 产品名称
    /// </summary>
    public string ProductName { get; set; }

    /// <summary>
    /// 产品类别
    /// </summary>
    public ProductType ProductType { get; set; }

    /// <summary>
    /// 到期时间
    /// </summary>
    public DateTime ExpiryTime { get; set; }

    /// <summary>
    /// 启用/禁用
    /// </summary>
    public bool Enable { get; set; }
}