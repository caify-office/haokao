using HaoKao.ProductService.Domain.Enums;

namespace HaoKao.ProductService.Application.ViewModels.StudentPermission;

/// <summary>
/// 创建学员权限
/// </summary>
/// <param name="SourceMode">来源</param>
/// <param name="StudentName">学员昵称(即用户昵称)</param>
/// <param name="PurchaseProductContents">购买的产品内容</param>
public record CreateStudentPermissionWebViewModel(
    SourceMode SourceMode,
    string StudentName,
    string PurchaseProductContents
) : IDto;