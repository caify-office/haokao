namespace HaoKao.BasicService.Application.ViewModels.Role;

/// <summary>
/// 角色用户操作模型
/// </summary>
public class EditRoleUserViewModel:IDto
{
    /// <summary>
    /// 用户ID列表
    /// </summary>
    [Required(ErrorMessage ="用户不能为空")]
    public IList<Guid> UserIds { get; set; } = new List<Guid>();
}