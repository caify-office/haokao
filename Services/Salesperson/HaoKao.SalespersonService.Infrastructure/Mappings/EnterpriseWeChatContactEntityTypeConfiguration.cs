using HaoKao.SalespersonService.Domain.Entities;

namespace HaoKao.SalespersonService.Infrastructure.Mappings;

public class EnterpriseWeChatContactEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<EnterpriseWeChatContact>
{
    public override void Configure(EntityTypeBuilder<EnterpriseWeChatContact> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<EnterpriseWeChatContact, Guid>(builder);

        builder.Property(x => x.FollowUserId).IsRequired().HasMaxLength(100).HasComment("添加了此外部联系人的企业成员Id");
        builder.Property(x => x.FollowUserName).IsRequired().HasMaxLength(100).HasComment("添加了此外部联系人的企业成员名称");
        builder.Property(x => x.Type).IsRequired().HasComment("联系人的类型，1表示该外部联系人是微信用户，2表示该外部联系人是企业微信用户");
        builder.Property(x => x.Name).IsRequired().HasMaxLength(100).HasComment("用户名称");
        builder.Property(x => x.UserId).IsRequired().HasMaxLength(100).HasComment("微信用户Id");
        builder.Property(x => x.UnionId).HasMaxLength(100).HasComment("微信unionid");
        builder.Property(x => x.CreateTime).IsRequired().HasComment("创建时间");

        builder.HasIndex(x => x.FollowUserId);
        builder.HasIndex(x => x.UnionId);
    }
}