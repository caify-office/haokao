namespace HaoKao.BasicService.Application.ViewModels.Role;

[AutoMapFrom(typeof(Domain.Entities.Role))]
public class RoleEditViewModel : IDto
{
    public Guid? Id { get; set; }

    [DisplayName("角色名称")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MinLength(1, ErrorMessage = "{0}最小长度为1")]
    [MaxLength(30, ErrorMessage = "{0}最大长度为30")]
    public string Name { get; set; }

    [DisplayName("角色描述")]
    [MaxLength(200, ErrorMessage = "{0}最大长度为200")]
    public string Desc { get; set; }
    public Guid[] UserIds { get; set; } = [];
}