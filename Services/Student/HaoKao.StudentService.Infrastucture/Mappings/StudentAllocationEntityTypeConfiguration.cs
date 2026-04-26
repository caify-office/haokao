using HaoKao.StudentService.Domain.Entities;

namespace HaoKao.StudentService.Infrastructure.Mappings;

public class StudentAllocationEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<StudentAllocation>
{
    public override void Configure(EntityTypeBuilder<StudentAllocation> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<StudentAllocation, Guid>(builder);

        builder.Property(x => x.StudentId).HasComment("学员Id");
        builder.Property(x => x.RegisterUserId).HasComment("注册用户Id");
        builder.Property(x => x.UnionId).IsRequired().HasMaxLength(100).HasComment("微信UnionId");
        builder.Property(x => x.SalespersonId).HasComment("销售人员Id");
        builder.Property(x => x.SalespersonName).IsRequired().HasMaxLength(100).HasComment("销售人员姓名");
        builder.Property(x => x.EnterpriseWeChatId).IsRequired().HasMaxLength(100).HasComment("企业微信Id");
        builder.Property(x => x.AllocationTime).HasComment("分配时间");
        builder.Property(x => x.Remark).HasComment("备注");
        builder.Property(x => x.TenantId).HasComment("租户Id");

        builder.HasIndex(x => new { x.StudentId, x.RegisterUserId, x.UnionId, x.TenantId }).IsUnique();
        builder.HasIndex(x => new { x.SalespersonName, x.AllocationTime });

        builder.HasOne(x => x.Student).WithOne().OnDelete(DeleteBehavior.Cascade);
    }
}