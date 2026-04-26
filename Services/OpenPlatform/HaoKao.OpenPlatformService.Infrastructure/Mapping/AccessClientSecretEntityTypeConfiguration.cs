namespace HaoKao.OpenPlatformService.Infrastructure.Mapping;

public class AccessClientSecretEntityTypeConfiguration: GirvsAbstractEntityTypeConfiguration<AccessClientSecret>
{
    public override void Configure(EntityTypeBuilder<AccessClientSecret> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<AccessClientSecret,Guid>(builder);
        builder.Property(x => x.Value).HasMaxLength(4000).IsRequired();
        builder.Property(x => x.Type).HasMaxLength(250).IsRequired();
        builder.Property(x => x.HashType).HasMaxLength(250).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(2000);

        //种子数据
        //client1-Secret
        builder.HasData(new AccessClientSecret
        {
            Id = Guid.Parse("6a8f88b5-4a49-4d32-56e6-c7198721f42e"),
            Value = "c20be2ef-8da4-4827-9671-35dcbb510441",
            Type = "SharedSecret",
            HashType = "Sha256",
           AccessClientId=Guid.Parse("2c939d03-564d-cfb5-7175-7065defd9dfb"),
        });
        //client2-Secret
        builder.HasData(new AccessClientSecret
        {
            Id = Guid.Parse("da2ebf42-a5c8-a4c7-f12f-0a0435b22516"),
            Value = "dd155ff4-69cd-47e0-b102-d90d90d770ed",
            Type = "SharedSecret",
            HashType = "Sha256",
            AccessClientId = Guid.Parse("a17c159c-3385-f479-e0bf-de9e9d480cfa"),
        });
        //client3-Secret
        builder.HasData(new AccessClientSecret
        {
            Id = Guid.Parse("cc65aceb-46d3-39a5-6513-c3ad4285b5dc"),
            Value = "e68d9858-5dfd-4e18-a160-bdf68c700bb0",
            Type = "SharedSecret",
            HashType = "Sha256",
            AccessClientId = Guid.Parse("bfbe1287-a91e-2c4b-7159-295168c17040"),
        });
        //client4-Secret
        builder.HasData(new AccessClientSecret
        {
            Id = Guid.Parse("39fe74c3-4dcd-2af3-045a-81d33138c27b"),
            Value = "41c76ec7-8d4e-4b92-a516-35104f2d9606",
            Type = "SharedSecret",
            HashType = "Sha256",
            AccessClientId = Guid.Parse("ec2bc16c-ecf9-4186-d344-71fc0f3e66f6"),
        });
    }
}