using HaoKao.BasicService.Domain.Entities;

namespace HaoKao.BasicService.Infrastructure.EntityConfigurations;

public class UserEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<User>
{
    public override void Configure(EntityTypeBuilder<User> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<User,Guid>(builder);
        builder.Property(x => x.UserAccount).HasColumnType("varchar(36)");
        builder.Property(x => x.UserPassword).HasColumnType("varchar(36)");
        builder.Property(x => x.UserName).HasColumnType("varchar(50)");
        builder.Property(x => x.ContactNumber).HasColumnType("varchar(12)");
        builder.Property(x => x.State).HasColumnType("int");
        builder.Property(x => x.UserType).HasColumnType("int");
        builder.Property(x => x.TenantName).HasColumnType("varchar(500)");

        //索引
        builder.HasIndex(x => new {x.UserAccount,x.TenantId}).IsUnique();

        //添加用户种子数据
        builder.HasData(new User
        {
            Id = Guid.Parse("58205e0e-1552-4282-bedc-a92d0afb37df"),
            UserName = "系统管理员",
            UserPassword = "21232F297A57A5A743894A0E4A801FC3",
            UserAccount = "systemsuperadmin",
            TenantId = Guid.Empty,
            TenantName = "系统管理",
            UserType = UserType.AdminUser,
            State = DataState.Enable,
            OtherId = Guid.Empty,
            IsInitData = true,
            CreatorId = Guid.Parse("58205e0e-1552-4282-bedc-a92d0afb37df")
        });
    }
}