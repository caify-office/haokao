using Girvs.EntityFrameworkCore.EntityConfigurations;
using HaoKao.OrderService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HaoKao.OrderService.Infrastructure.EntityConfigurations;

public class ProductSalesEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<ProductSales>
{
    public override void Configure(EntityTypeBuilder<ProductSales> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<ProductSales, Guid>(builder);

        builder.Property(x => x.ProductId).HasComment("产品Id");
        builder.Property(x => x.ProductName).HasColumnType("varchar").HasMaxLength(100).HasComment("产品名称");
        builder.Property(x => x.ActualPrice).HasPrecision(10, 2).HasComment("价格");
        builder.Property(x => x.CreateTime).HasComment("创建时间");
        builder.Property(x => x.TenantId).HasComment("租户Id");

        builder.HasIndex(x => new { x.ProductId, x.CreateTime, x.TenantId });
    }
}