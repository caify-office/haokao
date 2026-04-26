using HaoKao.ProductService.Domain.Entities;

namespace HaoKao.ProductService.Infrastructure.Mappings;

public class ProductPackageEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<ProductPackage>
{
    public override void Configure(EntityTypeBuilder<ProductPackage> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<ProductPackage, Guid>(builder);

        builder.Property(x => x.SimpleName)
               .HasColumnType("varchar")
               .HasMaxLength(50)
               .HasComment("简称");

        builder.Property(x => x.SalesRemind)
               .HasColumnType("text")
               .HasComment("售后提醒");

        builder.Property(x => x.Name)
               .HasColumnType("varchar")
               .HasMaxLength(100)
               .HasComment("名称");

        builder.Property(x => x.CardImage)
               .HasColumnType("varchar")
               .HasMaxLength(1000)
               .HasComment("产品卡片图片");

        builder.Property(x => x.DetailImage)
               .HasColumnType("varchar")
               .HasMaxLength(1000)
               .HasComment("详细介绍图片");
        builder.Property(x => x.ExpiryTime)
               .HasColumnType("datetime")
               .HasComment("到期时间");

        builder.Property(x => x.PreferentialExpiryTime)
               .HasColumnType("datetime")
               .HasComment("优惠到期时间");

        builder.Property(x => x.Year)
               .HasColumnType("datetime")
               .HasComment("所属年份");

        builder.Property(x => x.Selling)
               .HasColumnType("json")
               .HasConversion(
                   v => JsonConvert.SerializeObject(v),
                   v => v.IsNullOrEmpty()
                       ? new List<string>(0)
                       : JsonConvert.DeserializeObject<List<string>>(v)
               )
               .HasComment("卖点");

        builder.Property(x => x.FeaturedService)
               .HasColumnType("json")
               .HasConversion(
                   v => JsonConvert.SerializeObject(v),
                   v => v.IsNullOrEmpty()
                       ? new List<Guid>(0)
                       : JsonConvert.DeserializeObject<List<Guid>>(v)
               )
               .HasComment("特色服务");

        builder.Property(x => x.Detail)
               .HasColumnType("json")
               .HasConversion(
                   v => JsonConvert.SerializeObject(v),
                   v => v.IsNullOrEmpty()
                       ? new Dictionary<Guid, string>()
                       : JsonConvert.DeserializeObject<Dictionary<Guid, string>>(v)
               )
               .HasComment("对比详细介绍");

        builder.Property(x => x.ProductList)
               .HasColumnType("json")
               .HasConversion(
                   v => JsonConvert.SerializeObject(v),
                   v => v.IsNullOrEmpty()
                       ? new List<Guid>(0)
                       : JsonConvert.DeserializeObject<List<Guid>>(v)
               )
               .HasComment("对应的产品列表");

        builder.Property(x => x.LecturerList)
               .HasColumnType("json")
               .HasConversion(
                   v => JsonConvert.SerializeObject(v),
                   v => v.IsNullOrEmpty()
                       ? new List<Guid>(0)
                       : JsonConvert.DeserializeObject<List<Guid>>(v)
               )
               .HasComment("讲师列表");

        builder.Property(x => x.SimpleDesc)
               .HasColumnType("varchar")
               .HasMaxLength(100)
               .HasComment("简单描述");

        builder.Property(x => x.Desc)
               .HasColumnType("varchar")
               .HasMaxLength(1000)
               .HasComment("描述");


        builder.Property(x => x.IsExperience).HasDefaultValue(false).HasComment("是否体验产品包");

        builder.Property(x => x.IsSupportInstallmentPayment).HasDefaultValue(false).HasComment("是否支持分期支付");

        builder.Property(x => x.CreatorName).HasColumnType("varchar(40)").HasComment("创建人名称");

        builder.HasIndex(x =>
                             new
                             {
                                 x.ProductType,
                                 x.Year,
                                 x.Enable,
                                 x.Shelves,
                                 x.TenantId
                             });
    }
}