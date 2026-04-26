namespace HaoKao.OpenPlatformService.Infrastructure.Mapping;

public class AccessClientEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<AccessClient>
{
    public override void Configure(EntityTypeBuilder<AccessClient> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<AccessClient,Guid>(builder);

        builder.Property(x => x.ClientId).HasMaxLength(200).IsRequired();
        builder.Property(x => x.ProtocolType).HasMaxLength(200).IsRequired();
        builder.Property(x => x.ClientName).HasMaxLength(200);
        builder.Property(x => x.ClientUri).HasMaxLength(2000);
        builder.Property(x => x.LogoUri).HasMaxLength(2000);
        builder.Property(x => x.Description).HasMaxLength(1000);
        builder.Property(x => x.FrontChannelLogoutUri).HasMaxLength(2000);
        builder.Property(x => x.BackChannelLogoutUri).HasMaxLength(2000);
        builder.Property(x => x.ClientClaimsPrefix).HasMaxLength(200);
        builder.Property(x => x.PairWiseSubjectSalt).HasMaxLength(200);
        builder.Property(x => x.UserCodeType).HasMaxLength(100);
     

        builder.HasIndex(x => x.ClientId).IsUnique();

        builder
            .HasMany(x => x.AllowedGrantTypes)
            .WithOne(x => x.AccessClient)
            .HasForeignKey(x => x.AccessClientId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.RedirectUris)
            .WithOne(x => x.AccessClient)
            .HasForeignKey(x => x.AccessClientId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.PostLogoutRedirectUris)
            .WithOne(x => x.AccessClient)
            .HasForeignKey(x => x.AccessClientId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.AllowedScopes)
            .WithOne(x => x.AccessClient)
            .HasForeignKey(x => x.AccessClientId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);



        builder.HasMany(x => x.AllowedIdentityTokenSigningAlgorithms)
            .WithOne(x => x.AccessClient)
            .HasForeignKey(x => x.AccessClientId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.ClientSecrets)
            .WithOne(x => x.AccessClient)
            .HasForeignKey(x => x.AccessClientId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Claims)
            .WithOne(x => x.AccessClient)
            .HasForeignKey(x => x.AccessClientId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.IdentityProviderRestrictions)
            .WithOne(x => x.AccessClient)
            .HasForeignKey(x => x.AccessClientId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.AllowedCorsOrigins)
            .WithOne(x => x.AccessClient)
            .HasForeignKey(x => x.AccessClientId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Properties)
            .WithOne(x => x.AccessClient)
            .HasForeignKey(x => x.AccessClientId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasMany(x => x.DomainProxies)
            .WithOne(x => x.AccessClient)
            .HasForeignKey(x => x.AccessClientId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        //种子数据
        //client1
        builder.HasData(new AccessClient
        {
            Id = Guid.Parse("2c939d03-564d-cfb5-7175-7065defd9dfb"),
            ClientId = "acounts.haokao123.com.personal.client",
            ClientName = "好考教育平台-个人中心",
            ClientUri = "https://accounts.haokao123.com/personal",
            RequireConsent = false,
            AllowAccessTokensViaBrowser = true,
            AllowOfflineAccess = true,
            AccessTokenLifetime = 60 * 5,
        });
        //client2
        builder.HasData(new AccessClient
        {
            Id = Guid.Parse("a17c159c-3385-f479-e0bf-de9e9d480cfa"),
            ClientId = "haokao.questionbank.app.client",
            ClientName = "经济师慧题库手机App",
            ClientUri = "https://accounts.haokao123.com/app",
            RequireConsent = false,
            AllowAccessTokensViaBrowser = true,
            AllowOfflineAccess = true,
            AccessTokenLifetime = 60 * 60 * 24 * 30,
        });
        //client3
        builder.HasData(new AccessClient
        {
            Id = Guid.Parse("bfbe1287-a91e-2c4b-7159-295168c17040"),
            ClientId = "haokao.questionbank.wechat.mini.client",
            ClientName = "经济师慧题库微信小程序",
            ClientUri = "https://accounts.haokao123.com/app",
            RequireConsent = false,
            AllowAccessTokensViaBrowser = true,
            AllowOfflineAccess = true,
            AccessTokenLifetime = 60 * 60 * 24 * 30,
        });
        //client4
        builder.HasData(new AccessClient
        {
            Id = Guid.Parse("ec2bc16c-ecf9-4186-d344-71fc0f3e66f6"),
            ClientId = "haokao123.com.web.client",
            ClientName = "好考教育平台",
            ClientUri = "http://localhost:8088",
            RequireConsent = false,
            AllowAccessTokensViaBrowser = true,
            AllowOfflineAccess = true,
            AccessTokenLifetime = 60 * 5 * 12,
        });
    }
}