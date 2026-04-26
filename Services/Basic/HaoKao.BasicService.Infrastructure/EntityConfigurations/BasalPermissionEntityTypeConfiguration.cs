using HaoKao.BasicService.Domain.Entities;

namespace HaoKao.BasicService.Infrastructure.EntityConfigurations;

public class BasalPermissionEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<BasalPermission>
{
    public override void Configure(EntityTypeBuilder<BasalPermission> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<BasalPermission, Guid>(builder);
        builder.Property(x => x.AppliedObjectType).HasColumnType("int");
        builder.Property(x => x.ValidateObjectType).HasColumnType("int");
        builder.Property(x => x.DenyMask).HasColumnType("bigint");
        builder.Property(x => x.AllowMask).HasColumnType("bigint");
            
            
            
            
            
        //需要添加系统管理员的权限
        //索引 
        builder.HasIndex(x => x.AppliedObjectType);
        builder.HasIndex(x => x.ValidateObjectType);
        builder.HasIndex(x => x.AppliedId);
        builder.HasIndex(x => x.AppliedObjectId);
        builder.HasIndex(x => x.ValidateObjectId);
    }
}