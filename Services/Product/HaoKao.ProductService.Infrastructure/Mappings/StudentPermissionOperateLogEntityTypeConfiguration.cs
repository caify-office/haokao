using HaoKao.ProductService.Domain.Entities;

namespace HaoKao.ProductService.Infrastructure.Mappings;

public class StudentPermissionOperateLogEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<StudentPermissionOperateLog>
{
    public override void Configure(EntityTypeBuilder<StudentPermissionOperateLog> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<StudentPermissionOperateLog, Guid>(builder);

        builder.Property(x => x.StudentPermissionId).IsRequired().HasComment("学员权限Id");
        builder.Property(x => x.StudentName).HasMaxLength(50).IsRequired().HasComment("学员昵称(即用户昵称)");
        builder.Property(x => x.StudentId).IsRequired().HasComment("学员ID（即用户ID）");
        builder.Property(x => x.ProductId).IsRequired().HasComment("产品Id");
        builder.Property(x => x.ProductName).HasMaxLength(50).IsRequired().HasComment("产品名称");
        builder.Property(x => x.OldExpiredTime).HasComment("原过期时间").IsRequired(false);
        builder.Property(x => x.NewExpiredTime).IsRequired().HasComment("现过期时间");
        builder.Property(x => x.ProductType).HasComment("产品类型");
        builder.Property(x => x.CreateTime).HasComment("创建时间");
        builder.Property(x => x.CreatorId).HasComment("创建人Id");
        builder.Property(x => x.TenantId).HasComment("租户Id");

        builder.HasIndex(x => new { x.CreateTime, x.StudentName, });
    }
}