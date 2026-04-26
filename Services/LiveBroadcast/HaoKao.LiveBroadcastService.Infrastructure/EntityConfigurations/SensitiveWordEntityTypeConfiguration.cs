using HaoKao.LiveBroadcastService.Domain.Entities;

namespace HaoKao.LiveBroadcastService.Infrastructure.EntityConfigurations;

public class SensitiveWordEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<SensitiveWord>
{
    public override void Configure(EntityTypeBuilder<SensitiveWord> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<SensitiveWord, Guid>(builder);

        builder.Property(x => x.Content)
               .HasColumnType("text")
               .HasComment("内容");
    }
}