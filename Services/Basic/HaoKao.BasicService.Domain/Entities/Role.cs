namespace HaoKao.BasicService.Domain.Entities;

public class Role : AggregateRoot<Guid>, IIncludeInitField, IIncludeMultiTenant<Guid>
{
    public string Name { get; set; }

    public string Desc { get; set; }

    public bool IsInitData { get; set; }

    public Guid TenantId { get; set; }

    public virtual List<User> Users { get; set; } = [];
}