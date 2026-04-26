

using HaoKao.BurialPointService.Domain.Entities;

namespace HaoKao.BurialPointService.Infrastructure.EntityConfigurations;

public class BrowseRecordEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<BrowseRecord>
{
    public override void Configure(EntityTypeBuilder<BrowseRecord> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<BrowseRecord, Guid>(builder);

        builder.Property(x => x.UserName)
            .HasColumnType("varchar")
            .HasMaxLength(50)
            .HasComment("用户昵称");

        builder.Property(x => x.Phone)
            .HasColumnType("varchar")
            .HasMaxLength(50)
            .HasComment("手机号");

        builder.Property(x => x.BrowseData)
            .HasColumnType("json")
            .HasComment("浏览信息");

    }
}