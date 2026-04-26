using HaoKao.StudentService.Domain.Entities;

namespace HaoKao.StudentService.Infrastructure.Mappings;

public class UnionStudentEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<UnionStudent>
{
    public override void Configure(EntityTypeBuilder<UnionStudent> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<UnionStudent, Guid>(builder);

        builder.HasIndex(x => new { x.RegisterUserId, x.TenantId, x.IsPaidStudent });
    }
}