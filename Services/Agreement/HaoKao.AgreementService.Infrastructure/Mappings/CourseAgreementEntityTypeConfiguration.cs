using HaoKao.AgreementService.Domain.Entities;

namespace HaoKao.AgreementService.Infrastructure.Mappings;

public class CourseAgreementEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<CourseAgreement>
{
    public override void Configure(EntityTypeBuilder<CourseAgreement> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<CourseAgreement, Guid>(builder);

        builder.Property(x => x.Name)
               .HasColumnType("varchar")
               .HasMaxLength(50)
               .IsRequired()
               .HasComment("协议名称");

        builder.Property(x => x.Content)
               .HasColumnType("text")
               .IsRequired()
               .HasComment("协议内容");

        builder.Property(x => x.Continuation).HasComment("续读次数");
        builder.Property(x => x.AgreementType).HasComment("协议类型");
        builder.Property(x => x.TenantId).HasComment("租户Id");
        builder.Property(x => x.CreateTime).HasComment("创建时间");
        builder.Property(x => x.UpdateTime).HasComment("修改时间");
        builder.Property(x => x.CreatorId).HasComment("创建人");
    }
}