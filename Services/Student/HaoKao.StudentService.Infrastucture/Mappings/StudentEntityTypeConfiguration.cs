using HaoKao.StudentService.Domain.Entities;

namespace HaoKao.StudentService.Infrastructure.Mappings;

public class StudentEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<Student>
{
    public override void Configure(EntityTypeBuilder<Student> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<Student, Guid>(builder);

        builder.Property(x => x.RegisterUserId).IsRequired().HasComment("用户Id");
        builder.Property(x => x.IsPaidStudent).HasComment("是否付费学员");
        builder.Property(x => x.CreateTime).HasComment("创建时间");
        builder.Property(x => x.TenantId).IsRequired().HasComment("租户Id");

        builder.HasIndex(x => new { x.RegisterUserId, x.TenantId }).IsUnique();

        builder.HasMany(x => x.StudentFollows).WithOne().HasForeignKey(x => x.StudentId).OnDelete(DeleteBehavior.Cascade);
    }
}