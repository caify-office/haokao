using HaoKao.AnsweringQuestionService.Domain.Entities;
using System;

namespace HaoKao.AnsweringQuestionService.Infrastructure.EntityConfigurations;

public class AnsweringQuestionReplyEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<AnsweringQuestionReply>
{
    public override void Configure(EntityTypeBuilder<AnsweringQuestionReply> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<AnsweringQuestionReply, Guid>(builder);

        builder.Property(x => x.ReplyContent)
               .HasColumnType("longtext")
               .HasComment("答疑回复内容");

        builder.Property(x => x.ReplyUserName)
               .HasColumnType("varchar")
               .HasMaxLength(50)
               .HasComment("回复人用户名");

        builder.HasIndex(x => new { x.AnsweringQuestionId, x.TenantId });
    }
}