using HaoKao.ProductService.Domain.Entities;

namespace HaoKao.ProductService.Infrastructure.Mappings;

public class SupervisorClassEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<SupervisorClass>
{
    public override void Configure(EntityTypeBuilder<SupervisorClass> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<SupervisorClass, Guid>(builder);

        builder.Property(x => x.Name)
               .HasColumnType("varchar")
               .HasMaxLength(100)
               .HasComment("班级名称");

        builder.Property(x => x.ProductPackageName)
               .HasColumnType("varchar")
               .HasMaxLength(100)
               .HasComment("产品包名称");

        builder.Property(x => x.ProductName)
               .HasColumnType("varchar")
               .HasMaxLength(100)
               .HasComment("产品名称");

        builder.Property(x => x.SalespersonId)
               .HasColumnType("char(36)")
               .HasComment("营销人员Id");

        builder.Property(x => x.SalespersonName)
               .HasColumnType("varchar")
               .HasMaxLength(50)
               .HasComment("营销人员名称");

        builder
            .HasMany(x => x.SupervisorStudents)
            .WithOne(x => x.SupervisorClass)
            .HasForeignKey(x => x.SupervisorClassId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => new { x.ProductId, x.Year, x.TenantId });
    }
}