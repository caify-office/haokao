using HaoKao.LiveBroadcastService.Domain.Entities;

namespace HaoKao.LiveBroadcastService.Infrastructure.EntityConfigurations;

public class LiveAdministratorEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<LiveAdministrator>
{
    public override void Configure(EntityTypeBuilder<LiveAdministrator> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<LiveAdministrator, Guid>(builder);

        builder.Property(x => x.Name)
               .HasColumnType("varchar")
               .HasMaxLength(50)
               .HasComment("姓名");

        builder.Property(x => x.Phone)
               .HasColumnType("varchar")
               .HasMaxLength(50)
               .HasComment("手机号");

        builder.HasIndex(x => new {x.UserId, x.Phone, x.Name, });
    }
}