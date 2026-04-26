using HaoKao.TenantService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;

namespace HaoKao.TenantService.Infrastructure.Mappings;

public class TenantEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<Tenant>
{
    public override void Configure(EntityTypeBuilder<Tenant> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<Tenant, Guid>(builder);
        var converter = new ValueConverter<List<PaymentConfig>, string>(v => JsonConvert.SerializeObject(v), v => JsonConvert.DeserializeObject<List<PaymentConfig>>(v));
        builder.Property(x => x.OtherName).HasColumnType("varchar(500)").HasComment("其它名称");
        builder.Property(x => x.TenantName).HasColumnType("varchar(500)").HasComment("租户名称");
        builder.Property(x => x.TenantNo).HasColumnType("varchar(20)").HasComment("租户编号或代码");
        builder.Property(x => x.ReleaseState).HasColumnType("int").HasComment("发布状态");
        builder.Property(x => x.AnnualExamTime).HasColumnType("datetime").HasComment("年度考试时间");
        builder.Property(x => x.StartState).HasColumnType("int").HasComment("启用禁用状态");
        builder.Property(x => x.CreateTime).HasColumnType("datetime").HasComment("创建时间");
        builder.Property(x => x.Instructions).HasMaxLength(500).HasComment("说明");
        builder.Property(x => x.AdminUserAcount).HasColumnType("varchar(36)").HasComment("租户管理员账号");
        builder.Property(x => x.AdminUserName).HasColumnType("varchar(36)").HasComment("租户管理员名称");
        builder.Property(x => x.AdminPhone).HasColumnType("varchar(36)").HasComment("租户管理员手机号码");
        builder.Property(x => x.PaymentConfigs).HasColumnType("text").HasComment("收款配置").HasConversion(converter);
    }
}