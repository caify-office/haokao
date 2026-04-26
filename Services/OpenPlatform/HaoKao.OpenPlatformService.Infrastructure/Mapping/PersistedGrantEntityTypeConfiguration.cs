namespace HaoKao.OpenPlatformService.Infrastructure.Mapping;

public class PersistedGrantEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<PersistedGrant>
{
    public override void Configure(EntityTypeBuilder<PersistedGrant> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<PersistedGrant, Guid>(builder);

        builder.Property(x => x.Key).HasMaxLength(200);
        builder.Property(x => x.Type).HasMaxLength(50).IsRequired();
        builder.Property(x => x.SubjectId).HasMaxLength(200);
        builder.Property(x => x.SessionId).HasMaxLength(100);
        builder.Property(x => x.ClientId).HasMaxLength(200).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(200);
        builder.Property(x => x.CreationTime).IsRequired();

        // 50000 chosen to be explicit to allow enough size to avoid truncation, yet stay beneath the MySql row size limit of ~65K
        // apparently anything over 4K converts to nvarchar(max) on SqlServer
        builder.Property(x => x.Data).HasMaxLength(50000).IsRequired();

        builder.HasIndex(x => x.Key).IsUnique();
        builder.HasIndex(x => new { x.SubjectId, x.ClientId, x.SessionId });
        builder.HasIndex(x => new { x.ClientId, x.SessionId });
        builder.HasIndex(x => x.Expiration);
    }
}