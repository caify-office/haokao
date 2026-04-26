using HaoKao.AuditLogService.Domain.Entities;

namespace HaoKao.AuditLogService.Infrastructure.Mapping;

public class AuditLogEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<AuditLog>
{
    public override void Configure(EntityTypeBuilder<AuditLog> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<AuditLog, Guid>(builder);
        builder.Property(x => x.Message).HasColumnType("varchar(255)");
        builder.Property(x => x.MessageContent).HasColumnType("text");
        builder.Property(x => x.ServiceModuleName).HasColumnType("varchar(36)");
        builder.Property(x => x.TenantName).HasColumnType("varchar(100)");
        builder.Property(x => x.AddressIp).HasColumnType("varchar(100)");
        builder.Property(x => x.CreatorName).HasColumnType("varchar(40)");

        //索引
        builder.HasIndex(x => x.TenantId);

        builder.HasIndex(x => new
        {
            x.CreateTime,
            x.AddressIp,
            x.CreatorName,
            x.Message,
            x.TenantId,
            x.SourceType
        });
    }
}