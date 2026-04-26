using Girvs.EntityFrameworkCore.EntityConfigurations;
using HaoKao.OrderService.Domain.Entities;
using HaoKao.OrderService.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HaoKao.OrderService.Infrastructure.EntityConfigurations;

public class OrderInvoiceEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<OrderInvoice>
{
    public override void Configure(EntityTypeBuilder<OrderInvoice> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<OrderInvoice, Guid>(builder);

        builder.Property(x => x.OrderId).HasComment("订单Id");
        builder.Property(x => x.InvoiceType).HasComment("发票类型");
        builder.Property(x => x.VatInvoiceType).HasComment("增值税发票类型");
        builder.Property(x => x.InvoiceTitle).HasColumnType("varchar").HasMaxLength(100).HasComment("发票抬头");
        builder.Property(x => x.TaxNo).HasColumnType("varchar").HasMaxLength(100).HasComment("税号");
        builder.Property(x => x.RegistryAddress).HasColumnType("varchar").HasMaxLength(120).HasComment("注册地址");
        builder.Property(x => x.RegistryTel).HasColumnType("varchar").HasMaxLength(20).HasComment("注册电话");
        builder.Property(x => x.BankName).HasColumnType("varchar").HasMaxLength(100).HasComment("开户银行");
        builder.Property(x => x.BankAccount).HasColumnType("varchar").HasMaxLength(50).HasComment("银行账号");
        builder.Property(x => x.InvoiceFormat).HasComment("发票形式(获取方式)");
        builder.Property(x => x.Email).HasColumnType("varchar").HasMaxLength(100).HasComment("电子邮箱");
        builder.Property(x => x.ShippingArea).HasColumnType("varchar").HasMaxLength(150).HasComment("所在地区");
        builder.Property(x => x.ShippingAddress).HasColumnType("varchar").HasMaxLength(100).HasComment("收件人地址");
        builder.Property(x => x.ReceiverName).HasColumnType("varchar").HasMaxLength(50).HasComment("收件人姓名");
        builder.Property(x => x.ReceiverPhone).HasColumnType("varchar").HasMaxLength(20).HasComment("收件人联系电话");
        builder.Property(x => x.RequestState).HasDefaultValue(RequestState.Waiting).HasComment("申请状态");
        builder.Property(x => x.ShippingNumber).HasColumnType("varchar").HasMaxLength(50).HasComment("物流单号");
        builder.Property(x => x.LogisticsCompany).HasColumnType("varchar").HasMaxLength(50).HasComment("物流公司");
        builder.Property(x => x.ShippingTime).HasComment("发货时间");
        builder.Property(x => x.CreatorId).HasComment("创建者");
        builder.Property(x => x.CreateTime).HasComment("创建时间");
        builder.Property(x => x.TenantId).HasComment("租户Id");
        builder.Property(x => x.CreatorName).HasColumnType("varchar").HasMaxLength(50).HasComment("创建者名称");
        builder.Property(x => x.Remark).HasColumnType("varchar").HasMaxLength(200).HasComment("发票备注");

        // 配置 Order 与 OrderInvoice 的关系（例如一对一）
        builder
            .HasOne(oi => oi.Order) 
            .WithOne(o => o.OrderInvoice) 
            .HasForeignKey<OrderInvoice>(oi => oi.OrderId); // 显式指定外键为 OrderId（解决外键冲突

        builder.HasIndex(x => new { x.OrderId, x.TenantId });
    }
}