using HaoKao.ProductService.Domain.Entities;

namespace HaoKao.ProductService.Infrastructure.Mappings;

public class ProductEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<Product>
{
    public override void Configure(EntityTypeBuilder<Product> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<Product, Guid>(builder);

        builder.Property(x => x.Name)
               .HasMaxLength(100)
               .HasComment("产品名称");

        builder.Property(x => x.DisplayName)
               .HasMaxLength(100)
               .HasComment("显示名称");

        builder.Property(x => x.Description)
               .HasMaxLength(2000)
               .HasComment("产品描述");

        builder.Property(x => x.Icon)
               .HasMaxLength(1000)
               .HasComment("产品图标图片");

        builder.Property(x => x.DetailImage)
               .HasMaxLength(1000)
               .HasComment("产品详情介绍图片");


        builder.Property(x => x.Year)
               .HasMaxLength(4)
               .HasComment("年份");

        builder.Property(x => x.Price)
               .HasPrecision(6, 2)
               .HasComment("价格");

        builder.Property(x => x.DiscountedPrice)
               .HasPrecision(6, 2)
               .HasComment("优惠价格");

        builder.Property(x => x.AppleProductId)
               .HasMaxLength(36)
               .HasComment("苹果内购产品ID");

        builder.Property(x => x.GiveAwayAList)
               .HasColumnType("json")
               .HasConversion(
                   v => JsonConvert.SerializeObject(v),
                   v => v.IsNullOrEmpty()
                       ? new Dictionary<Guid, string>()
                       : JsonConvert.DeserializeObject<Dictionary<Guid, string>>(v)
               )
               .HasComment("赠送列表");
        builder.Property(x => x.IsExperience).HasDefaultValue(false).HasComment("是否体验产品");
        builder.HasMany(x => x.ProductPermissions)
               .WithOne()
               .HasForeignKey(x => x.ProductId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.AssistantProductPermissions)
               .WithOne()
               .HasForeignKey(x => x.ProductId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Cascade);

        builder.Property(x => x.CreatorName).HasColumnType("varchar(40)").HasComment("创建人名称");

        builder.HasIndex(x =>
                             new
                             {
                                 x.ProductType,
                                 x.Year,
                                 x.Enable,
                                 x.IsShelves,
                                 x.TenantId
                             });
    }
}