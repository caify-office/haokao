namespace HaoKao.OpenPlatformService.Infrastructure.Mapping;

public class AccessClientGrantTypeEntityTypeConfiguration: GirvsAbstractEntityTypeConfiguration<AccessClientGrantType>
{
    public override void Configure(EntityTypeBuilder<AccessClientGrantType> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<AccessClientGrantType,Guid>(builder);
        builder.Property(x => x.GrantType).HasMaxLength(250).IsRequired();

        //种子数据
        //client1-granttype
        builder.HasData(new AccessClientGrantType
        {
            Id = Guid.Parse("009f86bb-217a-c407-4c19-1fa1f5cb13ed"),
            GrantType = "implicit",
            AccessClientId = Guid.Parse("2c939d03-564d-cfb5-7175-7065defd9dfb"),
        });
        builder.HasData(new AccessClientGrantType
        {
            Id = Guid.Parse("17b85628-7ec5-87a8-7136-fd05298f5269"),
            GrantType = "password",
            AccessClientId = Guid.Parse("2c939d03-564d-cfb5-7175-7065defd9dfb"),
        });

        //client2-granttype
        builder.HasData(new AccessClientGrantType
        {
            Id = Guid.Parse("0b7cf34e-419a-1f6a-04bb-c95ad4d66a70"),
            GrantType = "implicit",
            AccessClientId = Guid.Parse("a17c159c-3385-f479-e0bf-de9e9d480cfa"),
        });
        builder.HasData(new AccessClientGrantType
        {
            Id = Guid.Parse("655fc79b-9570-0017-278b-d4621fe55158"),
            GrantType = "password",
            AccessClientId = Guid.Parse("a17c159c-3385-f479-e0bf-de9e9d480cfa"),
        });

        //client3-granttype
        builder.HasData(new AccessClientGrantType
        {
            Id = Guid.Parse("1f85560a-3376-ba7c-a507-53a3c493e1a6"),
            GrantType = "implicit",
            AccessClientId = Guid.Parse("bfbe1287-a91e-2c4b-7159-295168c17040"),
        });

        //client4-granttype
        builder.HasData(new AccessClientGrantType
        {
            Id = Guid.Parse("d781c6b6-4030-6ffc-a86e-48d8cc391607"),
            GrantType = "implicit",
            AccessClientId = Guid.Parse("ec2bc16c-ecf9-4186-d344-71fc0f3e66f6"),
        });
        builder.HasData(new AccessClientGrantType
        {
            Id = Guid.Parse("ea6a14d4-41d6-0fc3-2b73-fd1816de8f96"),
            GrantType = "password",
            AccessClientId = Guid.Parse("ec2bc16c-ecf9-4186-d344-71fc0f3e66f6"),
        });
    }
}