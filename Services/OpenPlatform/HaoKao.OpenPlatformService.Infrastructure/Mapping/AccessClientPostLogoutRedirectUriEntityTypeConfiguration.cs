namespace HaoKao.OpenPlatformService.Infrastructure.Mapping;

public class AccessClientPostLogoutRedirectUriEntityTypeConfiguration: GirvsAbstractEntityTypeConfiguration<AccessClientPostLogoutRedirectUri>
{
    public override void Configure(EntityTypeBuilder<AccessClientPostLogoutRedirectUri> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<AccessClientPostLogoutRedirectUri,Guid>(builder);
        builder.Property(x => x.PostLogoutRedirectUri).HasMaxLength(2000).IsRequired();

        //种子数据
        //client1-PostLogoutRedirectUri
        builder.HasData(new AccessClientPostLogoutRedirectUri
        {
            Id = Guid.Parse("1cb77e01-1700-cbaf-d2da-397e1cb90029"),
            PostLogoutRedirectUri = "https://accounts.haokao123.com/personal",
            AccessClientId = Guid.Parse("2c939d03-564d-cfb5-7175-7065defd9dfb"),
        });
        //client2-PostLogoutRedirectUri
        builder.HasData(new AccessClientPostLogoutRedirectUri
        {
            Id = Guid.Parse("0e05e819-d9ec-cf01-a19e-ab511b3b4191"),
            PostLogoutRedirectUri = "https://accounts.haokao123.com/app",
            AccessClientId = Guid.Parse("a17c159c-3385-f479-e0bf-de9e9d480cfa"),
        });
        //client3-PostLogoutRedirectUri
        builder.HasData(new AccessClientPostLogoutRedirectUri
        {
            Id = Guid.Parse("8629cdef-0b9e-91a5-8c15-088982c4e413"),
            PostLogoutRedirectUri = "https://accounts.haokao123.com/app",
            AccessClientId = Guid.Parse("bfbe1287-a91e-2c4b-7159-295168c17040"),
        });

        //client4-PostLogoutRedirectUri
        builder.HasData(new AccessClientPostLogoutRedirectUri
        {
            Id = Guid.Parse("63853332-ebaf-0f21-cb10-7669710e3270"),
            PostLogoutRedirectUri = "http://localhost:8088",
            AccessClientId = Guid.Parse("ec2bc16c-ecf9-4186-d344-71fc0f3e66f6"),
        });
    }
}