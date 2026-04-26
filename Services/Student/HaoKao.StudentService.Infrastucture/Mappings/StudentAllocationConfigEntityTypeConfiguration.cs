using System.Text.Json;
using HaoKao.StudentService.Domain.Entities;
using Microsoft.Extensions.Options;

namespace HaoKao.StudentService.Infrastructure.Mappings;

public class StudentAllocationConfigEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<StudentAllocationConfig>
{
    public override void Configure(EntityTypeBuilder<StudentAllocationConfig> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<StudentAllocationConfig, Guid>(builder);

        var options = EngineContext.Current.Resolve<IOptions<JsonSerializerOptions>>();
        builder.Property(x => x.Data).HasColumnType("json").IsRequired().HasComment("配置数据").HasConversion(
            v => JsonSerializer.Serialize(v, options.Value),
            v => JsonSerializer.Deserialize<HashSet<PercentageAllocation>>(v, options.Value)
        );
        builder.Property(x => x.WaysOfAllocation).HasComment("分配方式");
        builder.Property(x => x.UpdateTime).HasComment("修改时间");
        builder.Property(x => x.TenantId).HasComment("租户Id");

        builder.HasIndex(x => x.TenantId).IsUnique();
    }
}