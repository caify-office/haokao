using HaoKao.ProductService.Domain.Entities;

namespace HaoKao.ProductService.Infrastructure.Mappings;

public class RelatedProductEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<RelatedProduct>
{
    public override void Configure(EntityTypeBuilder<RelatedProduct> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<RelatedProduct, Guid>(builder);

        builder.Property(x => x.ProductName)
               .HasColumnType("varchar")
               .HasMaxLength(50)
               .HasComment("产品名称");

        builder.Property(x => x.RelatedTargetProducName)
               .HasColumnType("varchar")
               .HasMaxLength(50)
               .HasComment("关联对象产品名称");

        builder.Property(x => x.CreatorName).HasColumnType("varchar").HasMaxLength(50).HasComment("创建人名称");
    }
}