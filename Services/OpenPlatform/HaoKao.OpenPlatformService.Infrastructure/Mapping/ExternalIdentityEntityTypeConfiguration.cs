using Girvs.Extensions;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;

namespace HaoKao.OpenPlatformService.Infrastructure.Mapping;

public class ExternalIdentityEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<ExternalIdentity>
{
    public override void Configure(EntityTypeBuilder<ExternalIdentity> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<ExternalIdentity, Guid>(builder);

        var otherConverter = new ValueConverter<Dictionary<string, string>, string>(
            v => JsonConvert.SerializeObject(v),
            v => (v.IsNullOrEmpty()
                ? new Dictionary<string, string>()
                : JsonConvert.DeserializeObject<Dictionary<string, string>>(v))!
        );

        builder.Property(x => x.Scheme).HasColumnType("varchar(20)").HasComment("平台名称");
        builder.Property(x => x.UniqueIdentifier).HasColumnType("varchar(100)").HasComment("唯一标识");
        builder.Property(x => x.OtherInformation).HasColumnType("varchar(1000)").HasComment("其它信息")
            .HasConversion(otherConverter);

        builder.HasIndex(x => new {x.UniqueIdentifier, x.Scheme}).IsUnique();
    }
}