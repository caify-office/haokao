namespace HaoKao.BasicService.Application.ViewModels.Role;

[AutoMapFrom(typeof(Domain.Entities.Role))]
public class RoleDetailViewModel : IDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Desc { get; set; }
    public Guid[] UserIds { get; set; } = [];
}