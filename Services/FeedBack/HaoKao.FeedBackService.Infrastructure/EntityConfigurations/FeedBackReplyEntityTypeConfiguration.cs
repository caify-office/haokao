using HaoKao.FeedBackService.Domain.Entities;
using System;

namespace HaoKao.FeedBackService.Infrastructure.EntityConfigurations;

public class FeedBackReplyEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<FeedBackReply>
{
    public override void Configure(EntityTypeBuilder<FeedBackReply> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<FeedBackReply, Guid>(builder);

        builder.Property(x => x.ReplyContent)
               .HasColumnType("varchar")
               .HasMaxLength(50)
               .HasComment("答疑回复内容");

        builder.Property(x => x.ReplyUserName)
               .HasColumnType("varchar")
               .HasMaxLength(50)
               .HasComment("回复人用户名");

        builder.Property(x => x.CreatorName).HasColumnType("varchar(40)");
    }
}