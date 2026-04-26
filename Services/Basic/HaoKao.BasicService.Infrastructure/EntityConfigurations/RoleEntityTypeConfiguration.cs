using HaoKao.BasicService.Domain.Entities;

namespace HaoKao.BasicService.Infrastructure.EntityConfigurations;

public class RoleEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<Role>
{
    public override void Configure(EntityTypeBuilder<Role> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<Role, Guid>(builder);
        builder.Property(x => x.Name).HasColumnType("varchar(30)").HasComment("角色名称");
        builder.Property(x => x.Desc).HasColumnType("varchar(200)").HasComment("角色描述");

        //索引
        builder.HasData(new Role
        {
            Id = Guid.Parse("70ecc373-16f7-42e9-b31b-e80507b7c20a"),
            Name = "考试管理员角色",
            Desc = "考试管理员具有的该角色",
            TenantId = Guid.Empty,
            IsInitData = true
        });
    }
}