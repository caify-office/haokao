using HaoKao.AnsweringQuestionService.Domain.Entities;
using System;

namespace HaoKao.AnsweringQuestionService.Infrastructure.EntityConfigurations;

public class AnsweringQuestionEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<AnsweringQuestion>
{
    public override void Configure(EntityTypeBuilder<AnsweringQuestion> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<AnsweringQuestion, Guid>(builder);

        builder.Property(x => x.UserName)
               .HasColumnType("varchar")
               .HasMaxLength(50)
               .HasComment("用户名称");

        builder.Property(x => x.Phone)
               .HasColumnType("varchar")
               .HasMaxLength(50)
               .HasComment("手机号码");

        builder.Property(x => x.SubjectName)
               .HasColumnType("varchar")
               .HasMaxLength(50)
               .HasComment("科目名称");

        builder.Property(x => x.BookPageSize)
               .HasColumnType("varchar")
               .HasMaxLength(50)
               .HasComment("书籍页码");

        builder.Property(x => x.BookName)
               .HasColumnType("varchar")
               .HasMaxLength(50)
               .HasComment("书籍名称以及相关描述");

        builder.Property(x => x.Description)
               .HasColumnType("varchar")
               .HasMaxLength(500)
               .HasComment("问题描述");

        builder.Property(x => x.Remark)
               .HasColumnType("longtext").HasComment("详细描述");


        builder.Property(x => x.FileUrl)
               .HasColumnType("text")
               .HasComment("上传的图片路径");

        builder.HasIndex(x => new { x.SubjectName, x.Phone, x.UserName, x.TenantId });
    }
}