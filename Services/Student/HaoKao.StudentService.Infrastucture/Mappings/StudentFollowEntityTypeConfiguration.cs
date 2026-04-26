using HaoKao.StudentService.Domain.Entities;

namespace HaoKao.StudentService.Infrastructure.Mappings;

public class StudentFollowEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<StudentFollow>
{
    public override void Configure(EntityTypeBuilder<StudentFollow> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<StudentFollow, Guid>(builder);

        builder.Property(x => x.StudentId).HasComment("学员Id");
        builder.Property(x => x.RegisterUserId).HasComment("用户Id");
        builder.Property(x => x.UnionId).HasMaxLength(100).HasComment("UnionId").HasComment("微信UnionId");
        builder.Property(x => x.SalespersonId).HasComment("销售人员Id");
        builder.Property(x => x.SalespersonName).IsRequired().HasMaxLength(100).HasComment("销售人员姓名");
        builder.Property(x => x.EnterpriseWeChatId).IsRequired().HasMaxLength(100).HasComment("企业微信Id");
        builder.Property(x => x.CreateTime).HasComment("添加时间");
        builder.Property(x => x.TenantId).HasComment("租户Id");

        builder.HasIndex(x => new { x.SalespersonName, x.TenantId });
    }
}