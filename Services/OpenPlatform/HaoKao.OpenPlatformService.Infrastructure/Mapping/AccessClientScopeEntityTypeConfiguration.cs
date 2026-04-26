namespace HaoKao.OpenPlatformService.Infrastructure.Mapping;

public class AccessClientScopeEntityTypeConfiguration: GirvsAbstractEntityTypeConfiguration<AccessClientScope>
{
    public override void Configure(EntityTypeBuilder<AccessClientScope> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<AccessClientScope,Guid>(builder);
        builder.Property(x => x.Scope).HasMaxLength(200).IsRequired();

        //种子数据
        //client1-Scope
        builder.HasData(new AccessClientScope
        {
            Id = Guid.Parse("bd30467a-b144-4377-cb96-0c9c6d0eb1ca"),
            Scope = "openid",
            AccessClientId = Guid.Parse("2c939d03-564d-cfb5-7175-7065defd9dfb"),
        });

        builder.HasData(new AccessClientScope
        {
            Id = Guid.Parse("c8a65d32-db3a-257d-8e4b-168a232dbbbf"),
            Scope = "phone",
            AccessClientId = Guid.Parse("2c939d03-564d-cfb5-7175-7065defd9dfb"),
        });
        builder.HasData(new AccessClientScope
        {
            Id = Guid.Parse("ebbcca29-684f-1b08-0871-e7f141dc3132"),
            Scope = "profile",
            AccessClientId = Guid.Parse("2c939d03-564d-cfb5-7175-7065defd9dfb"),
        });
        builder.HasData(new AccessClientScope
        {
            Id = Guid.Parse("1deeb5da-7c99-176c-3cdf-207ed1d1bc80"),
            Scope = "email",
            AccessClientId = Guid.Parse("2c939d03-564d-cfb5-7175-7065defd9dfb"),
        });
        builder.HasData(new AccessClientScope
        {
            Id = Guid.Parse("81776cff-fe59-2f0b-c5a2-8911ad1c0fcb"),
            Scope = "apiservice",
            AccessClientId = Guid.Parse("2c939d03-564d-cfb5-7175-7065defd9dfb"),
        });
        //client2-Scope
        builder.HasData(new AccessClientScope
        {
            Id = Guid.Parse("3eac9b91-17b6-7f99-ad8c-9afb425cf909"),
            Scope = "offline_access",
            AccessClientId = Guid.Parse("a17c159c-3385-f479-e0bf-de9e9d480cfa"),
        });
        builder.HasData(new AccessClientScope
        {
            Id = Guid.Parse("c955cf45-88b2-dc77-530f-3042a59128cb"),
            Scope = "openid",
            AccessClientId = Guid.Parse("a17c159c-3385-f479-e0bf-de9e9d480cfa"),
        });

        builder.HasData(new AccessClientScope
        {
            Id = Guid.Parse("76f15039-662d-0bff-73a4-ba15e6538b7b"),
            Scope = "phone",
            AccessClientId = Guid.Parse("a17c159c-3385-f479-e0bf-de9e9d480cfa"),
        });
        builder.HasData(new AccessClientScope
        {
            Id = Guid.Parse("3ff9b328-8328-5113-fca2-0419ccccf86d"),
            Scope = "profile",
            AccessClientId = Guid.Parse("a17c159c-3385-f479-e0bf-de9e9d480cfa"),
        });
       
        builder.HasData(new AccessClientScope
        {
            Id = Guid.Parse("d72257e1-e21c-a9a7-1753-6e1b1624d15d"),
            Scope = "apiservice",
            AccessClientId = Guid.Parse("a17c159c-3385-f479-e0bf-de9e9d480cfa"),
        });

        //client3-Scope
        builder.HasData(new AccessClientScope
        {
            Id = Guid.Parse("ae2629df-5292-70b5-1355-fd49741ba1ac"),
            Scope = "offline_access",
            AccessClientId = Guid.Parse("bfbe1287-a91e-2c4b-7159-295168c17040"),
        });
        builder.HasData(new AccessClientScope
        {
            Id = Guid.Parse("2801d56e-274e-af54-d220-e755f65c78c1"),
            Scope = "openid",
            AccessClientId = Guid.Parse("bfbe1287-a91e-2c4b-7159-295168c17040"),
        });
        builder.HasData(new AccessClientScope
        {
            Id = Guid.Parse("6607a211-8166-9e62-69c3-7a6190df0b3a"),
            Scope = "profile",
            AccessClientId = Guid.Parse("bfbe1287-a91e-2c4b-7159-295168c17040"),
        });
        builder.HasData(new AccessClientScope
        {
            Id = Guid.Parse("f6c3f7cb-f474-3647-4d05-b8b2b18d934b"),
            Scope = "phone",
            AccessClientId = Guid.Parse("bfbe1287-a91e-2c4b-7159-295168c17040"),
        });
        builder.HasData(new AccessClientScope
        {
            Id = Guid.Parse("52c3e1fe-4488-6515-74e5-af30d96836c4"),
            Scope = "apiservice",
            AccessClientId = Guid.Parse("bfbe1287-a91e-2c4b-7159-295168c17040"),
        });

        //client4-Scope
        builder.HasData(new AccessClientScope
        {
            Id = Guid.Parse("d71b85a7-70a7-28b7-3b4c-e494af940b97"),
            Scope = "offline_access",
            AccessClientId = Guid.Parse("ec2bc16c-ecf9-4186-d344-71fc0f3e66f6"),
        });
        builder.HasData(new AccessClientScope
        {
            Id = Guid.Parse("63027529-dcae-9dfa-ae48-07d315299b0b"),
            Scope = "openid",
            AccessClientId = Guid.Parse("ec2bc16c-ecf9-4186-d344-71fc0f3e66f6"),
        });
        builder.HasData(new AccessClientScope
        {
            Id = Guid.Parse("d822e094-8445-091d-2cf8-eff3318e37cb"),
            Scope = "profile",
            AccessClientId = Guid.Parse("ec2bc16c-ecf9-4186-d344-71fc0f3e66f6"),
        });
        builder.HasData(new AccessClientScope
        {
            Id = Guid.Parse("7595011c-7fad-2313-631d-49748284451d"),
            Scope = "phone",
            AccessClientId = Guid.Parse("ec2bc16c-ecf9-4186-d344-71fc0f3e66f6"),
        });
        builder.HasData(new AccessClientScope
        {
            Id = Guid.Parse("9e556e4b-e3c1-d50e-2cff-937a66b1b38d"),
            Scope = "apiservice",
            AccessClientId = Guid.Parse("ec2bc16c-ecf9-4186-d344-71fc0f3e66f6"),
        });
    }
}