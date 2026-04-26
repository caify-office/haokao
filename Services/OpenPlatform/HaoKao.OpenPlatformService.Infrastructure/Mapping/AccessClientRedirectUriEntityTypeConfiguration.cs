namespace HaoKao.OpenPlatformService.Infrastructure.Mapping;

public class AccessClientRedirectUriEntityTypeConfiguration: GirvsAbstractEntityTypeConfiguration<AccessClientRedirectUri>
{
    public override void Configure(EntityTypeBuilder<AccessClientRedirectUri> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<AccessClientRedirectUri,Guid>(builder);
        builder.Property(x => x.RedirectUri).HasMaxLength(2000).IsRequired();

        //种子数据
        //client1-RedirectUri
        builder.HasData(new AccessClientRedirectUri
        {
            Id = Guid.Parse("d0aba72c-b9bc-aeaa-9896-896058cb995f"),
            RedirectUri = "https://accounts.haokao123.com/personal/oidc/signin-oidc",
            AccessClientId = Guid.Parse("2c939d03-564d-cfb5-7175-7065defd9dfb"),
        });
        builder.HasData(new AccessClientRedirectUri
        {
            Id = Guid.Parse("1c19262f-631c-b9d3-902d-138e71bf0074"),
            RedirectUri = "https://accounts.haokao123.com/personal/oidc/redirect-silentrenew",
            AccessClientId = Guid.Parse("2c939d03-564d-cfb5-7175-7065defd9dfb"),
        });
        //client2-RedirectUri
        builder.HasData(new AccessClientRedirectUri
        {
            Id = Guid.Parse("a5afcb17-33eb-4e2c-814b-ccd09cdf40df"),
            RedirectUri = "https://accounts.haokao123.com/app/oidc/signin-oidc",
            AccessClientId = Guid.Parse("a17c159c-3385-f479-e0bf-de9e9d480cfa"),
        });
        builder.HasData(new AccessClientRedirectUri
        {
            Id = Guid.Parse("b3c05bd8-9123-a64f-8451-23dde1e93269"),
            RedirectUri = "https://accounts.haokao123.com/app/oidc/redirect-silentrenew",
            AccessClientId = Guid.Parse("a17c159c-3385-f479-e0bf-de9e9d480cfa"),
        });
        //client3-RedirectUri
        builder.HasData(new AccessClientRedirectUri
        {
            Id = Guid.Parse("3daa2b34-874b-55d0-617a-79cd92c2690c"),
            RedirectUri = "https://accounts.haokao123.com/app/oidc/signin-oidc",
            AccessClientId = Guid.Parse("bfbe1287-a91e-2c4b-7159-295168c17040"),
        });
        builder.HasData(new AccessClientRedirectUri
        {
            Id = Guid.Parse("ee580b0c-78b7-28cb-e0f6-b628a7084e68"),
            RedirectUri = "https://accounts.haokao123.com/app/oidc/redirect-silentrenew",
            AccessClientId = Guid.Parse("bfbe1287-a91e-2c4b-7159-295168c17040"),
        });
        //client4-RedirectUri
        builder.HasData(new AccessClientRedirectUri
        {
            Id = Guid.Parse("60b2cc68-a697-8daa-d7f0-201bc0467c88"),
            RedirectUri = "http://localhost:8088/signinOidc",
            AccessClientId = Guid.Parse("ec2bc16c-ecf9-4186-d344-71fc0f3e66f6"),
        });
        builder.HasData(new AccessClientRedirectUri
        {
            Id = Guid.Parse("7e63b742-ccd2-c34a-5cc8-e04661521bbe"),
            RedirectUri = "http://localhost:8088/redirectSilentreneww",
            AccessClientId = Guid.Parse("ec2bc16c-ecf9-4186-d344-71fc0f3e66f6"),
        });
    }
}