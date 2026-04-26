using HaoKao.ProductService.Domain.Enums;

namespace HaoKao.ProductService.Domain.Commands.StudentPermission;

/// <summary>
/// 事件创建学员权限表命令
/// </summary>
/// <param name="StudentName">学员昵称(即用户昵称)</param>
/// <param name="StudentId">学员ID（即用户ID）</param>
/// <param name="OrderNumber">对应的订单号</param>
/// <param name="PurchaseProductContents">购买的产品内容</param>
/// <param name="SourceMode">来源</param>
/// <param name="Enable">启用/禁用</param>
public record CreateStudentPermissionEventCommand(
    string StudentName,
    Guid StudentId,
    string OrderNumber,
    string PurchaseProductContents,
    SourceMode SourceMode,
    bool Enable = true
) : Command("事件创建学员权限表");

/// <summary>
/// 购买的产品内容
/// </summary>
public class PurchaseProductContent
{
    /// <summary>
    /// 内容Id
    /// </summary>
    public Guid ContentId { get; set; }

    /// <summary>
    /// 内容名称
    /// </summary>
    public string ContentName { get; set; }

    /// <summary>
    /// 苹果内购产品Id
    /// </summary>
    public string AppleInPurchaseProductId { get; set; }

    /// <summary>
    /// 到期时间
    /// </summary>
    public DateTime ExpiryTime { get; set; }
}