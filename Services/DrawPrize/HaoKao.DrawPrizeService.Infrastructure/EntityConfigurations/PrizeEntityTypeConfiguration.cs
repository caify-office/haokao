using HaoKao.DrawPrizeService.Domain.Entities;

namespace HaoKao.DrawPrizeService.Infrastructure.EntityConfigurations;

public class PrizeEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<Prize>
{
    public override void Configure(EntityTypeBuilder<Prize> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<Prize, Guid>(builder);

        builder.Property(x => x.Name)
            .HasColumnType("varchar")
            .HasMaxLength(50)
            .HasComment("名称");

        var designatedStudentsConverter = new ValueConverter<List<DesignatedStudent>, string>(
            v => JsonConvert.SerializeObject(v),
            v => v.IsNullOrEmpty() ? new List<DesignatedStudent>() : JsonConvert.DeserializeObject<List<DesignatedStudent>>(v)
        );

        builder.Property(x => x.DesignatedStudents)
            .HasColumnType("json")
            .HasConversion(designatedStudentsConverter)
        .HasComment("指定学员");

        builder.HasIndex(x => x.DrawPrizeId);

        builder.Property(p => p.Version)
            .IsRowVersion(); // 声明 Version 属性为 RowVersion

    }
}