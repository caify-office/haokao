namespace HaoKao.BasicService.Application.ViewModels.User;

/// <summary>
/// 用户角色操作模型
/// </summary>
public class EditUserRoleViewModel : IDto
{
    /// <summary>
    /// 角色Id列表
    /// </summary>
    public IList<Guid> RoleIds { get; set; }
}