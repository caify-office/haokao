namespace HaoKao.OpenPlatformService.Infrastructure.Mapping;

public class AccessClientCorsOriginEntityTypeConfiguration:  GirvsAbstractEntityTypeConfiguration<AccessClientCorsOrigin>
{
    public override void Configure(EntityTypeBuilder<AccessClientCorsOrigin> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<AccessClientCorsOrigin,Guid>(builder);
        builder.Property(x => x.Origin).HasMaxLength(150).IsRequired();

        //种子数据
        //client1-Origin
        builder.HasData(new AccessClientCorsOrigin
        {
            Id = Guid.Parse("8805397e-da12-d03e-24f9-af00fe141283"),
            Origin = "https://accounts.haokao123.com",
            AccessClientId = Guid.Parse("2c939d03-564d-cfb5-7175-7065defd9dfb"),
        });
        //client2-Origin
        builder.HasData(new AccessClientCorsOrigin
        {
            Id = Guid.Parse("33567974-26e2-0717-c6b4-bb51fc905575"),
            Origin = "https://accounts.haokao123.com",
            AccessClientId = Guid.Parse("a17c159c-3385-f479-e0bf-de9e9d480cfa"),
        });

        //client3-Origin
        builder.HasData(new AccessClientCorsOrigin
        {
            Id = Guid.Parse("93188728-9c05-ccd3-ac7e-19eb703cea38"),
            Origin = "https://accounts.haokao123.com",
            AccessClientId = Guid.Parse("bfbe1287-a91e-2c4b-7159-295168c17040"),
        });

        //client4-Origin
        builder.HasData(new AccessClientCorsOrigin
        {
            Id = Guid.Parse("61e23568-271c-7ed3-e2e1-bab55ec6c949"),
            Origin = "http://localhost:8088",
            AccessClientId = Guid.Parse("ec2bc16c-ecf9-4186-d344-71fc0f3e66f6"),
        });


    }
}