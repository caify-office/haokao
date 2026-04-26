using HaoKao.LiveBroadcastService.Domain.Entities;

namespace HaoKao.LiveBroadcastService.Infrastructure.EntityConfigurations;

public class MutedUserEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<MutedUser>
{
    public override void Configure(EntityTypeBuilder<MutedUser> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<MutedUser, Guid>(builder);

        builder.Property(x => x.UserName)
               .HasColumnType("varchar")
               .HasMaxLength(50)
               .HasComment("用户名称");

        builder.Property(x => x.Phone)
               .HasColumnType("varchar")
               .HasMaxLength(50)
               .HasComment("手机号");

        builder.Property(x => x.CreatorName).HasColumnType("varchar(40)");

        builder.HasIndex(x => new { x.UserId, x.TenantId, });
    }
}