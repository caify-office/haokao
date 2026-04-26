using HaoKao.AgreementService.Domain.Entities;

namespace HaoKao.AgreementService.Infrastructure.Mappings;

public class StudentAgreementEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<StudentAgreement>
{
    public override void Configure(EntityTypeBuilder<StudentAgreement> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<StudentAgreement, Guid>(builder);

        builder.Property(x => x.ProductName)
               .HasColumnType("varchar")
               .HasMaxLength(50)
               .IsRequired()
               .HasComment("产品名称");

        builder.Property(x => x.AgreementName)
               .HasColumnType("varchar")
               .HasMaxLength(50)
               .IsRequired()
               .HasComment("协议名称");

        builder.Property(x => x.StudentName)
               .HasColumnType("varchar")
               .HasMaxLength(50)
               .IsRequired()
               .HasComment("学员名称");

        builder.Property(x => x.IdCard)
               .HasColumnType("varchar")
               .HasMaxLength(18)
               .IsRequired()
               .HasComment("身份证号");

        builder.Property(x => x.Contact)
               .HasColumnType("varchar")
               .HasMaxLength(20)
               .IsRequired()
               .HasComment("联系电话");

        builder.Property(x => x.Address)
               .HasColumnType("varchar")
               .HasMaxLength(50)
               .IsRequired()
               .HasComment("联系地址");

        builder.Property(x => x.Email)
               .HasColumnType("varchar")
               .HasMaxLength(50)
               .IsRequired()
               .HasComment("电子邮箱");

        builder.Property(x => x.ProductId).HasComment("产品Id");
        builder.Property(x => x.AgreementId).HasComment("协议Id");
        builder.Property(x => x.TenantId).HasComment("租户Id");
        builder.Property(x => x.CreateTime).HasComment("创建时间");
        builder.Property(x => x.UpdateTime).HasComment("修改时间");
        builder.Property(x => x.CreatorId).HasComment("签署人Id");

        builder.HasIndex(x => new { x.ProductId, x.AgreementId, x.CreatorId, x.TenantId });
    }
}