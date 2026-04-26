using Girvs.EntityFrameworkCore.EntityConfigurations;
using HaoKao.OrderService.Domain.Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace HaoKao.OrderService.Infrastructure.EntityConfigurations;

public class OrderEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<Order>
{
    public override void Configure(EntityTypeBuilder<Order> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<Order, Guid>(builder);

        var purchaseProductContentConverter = new ValueConverter<List<PurchaseProductContent>, string>(
            v => JsonConvert.SerializeObject(v),
            v => v.IsNullOrEmpty()
                ? new List<PurchaseProductContent>()
                : JsonConvert.DeserializeObject<List<PurchaseProductContent>>(v)
        );

        builder.Property(x => x.PlatformPayerName)
               .HasColumnType("varchar")
               .HasMaxLength(50)
               .HasComment("使用的平台配置的支付者的名称");

        builder.Property(x => x.OrderNumber)
               .HasColumnType("varchar")
               .HasMaxLength(50)
               .HasComment("订单号");

        builder.Property(x => x.OrderSerialNumber)
               .HasColumnType("varchar")
               .HasMaxLength(32)
               .HasComment("订单第三方流水号");

        builder.Property(x => x.PurchaseName)
               .HasColumnType("varchar")
               .HasMaxLength(100)
               .HasComment("购买产品名称");

        builder.Property(x => x.OrderAmount)
               .HasPrecision(10, 2)
               .HasComment("订单金额");

        builder.Property(x => x.ActualAmount)
               .HasPrecision(10, 2)
               .HasComment("实际金额");

        builder.Property(x => x.Phone)
                .HasColumnType("varchar(11)")
                .HasComment("手机号码");

        builder.Property(x => x.PurchaseProductContents)
               .HasColumnType("json")
               .HasConversion(purchaseProductContentConverter)
               .HasComment("购买商品详细内容")
                .Metadata.SetValueComparer(
            new ValueComparer<List<PurchaseProductContent>>(
                (List<PurchaseProductContent>? c1, List<PurchaseProductContent>? c2) => c1.SequenceEqual(c2), // 比较集合内容
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())), // 计算哈希
                c => c.ToList() // 克隆集合
            )
        );

        builder.Property(x => x.ClientName)
               .HasColumnType("varchar")
               .HasMaxLength(50)
               .HasComment("购买商品详细内容");

        builder.Property(x => x.ClientId)
               .HasColumnType("varchar")
               .HasMaxLength(50)
               .HasComment("客户端ID");

        builder.Property(x => x.CreatorName)
               .HasColumnType("varchar")
               .HasMaxLength(50)
               .HasComment("创建者名称");


        builder.HasIndex(x => x.OrderNumber).IsUnique();
        builder.HasIndex(x => x.OrderSerialNumber).IsUnique();
        builder.HasIndex(x => new
        {
            x.OrderNumber,
            x.PurchaseName,
            x.CreatorName,
            x.CreateTime,
            x.UpdateTime,
            x.OrderState,
            x.PurchaseProductType,
            x.TenantId
        });
    }
}