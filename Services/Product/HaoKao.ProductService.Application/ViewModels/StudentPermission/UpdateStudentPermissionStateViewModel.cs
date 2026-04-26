namespace HaoKao.ProductService.Application.ViewModels.StudentPermission;

[AutoMapTo(typeof(Domain.Entities.StudentPermission))]
public class UpdateStudentPermissionStateViewModel : IDto
{
    /// <summary>
    /// 启用/禁用
    /// </summary>
    [DisplayName("启用/禁用")]
    [Required(ErrorMessage = "{0}不能为空")]
    public bool Enable { get; set; }
}