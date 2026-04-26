using Girvs.Extensions;
using HaoKao.ChapterNodeService.Domain.ChapterNodeModule;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HaoKao.ChapterNodeService.Infrastructure.ChapterNodeModule;

public class ChapterNodeEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<ChapterNode>
{
    public override void Configure(EntityTypeBuilder<ChapterNode> builder)
    {
        var listConverter = new ValueConverter<List<Guid>, string>(
            v => string.Join(',', v),
            v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(s => s.ToGuid()).ToList()
        );

        OnModelCreatingBaseEntityAndTableKey<ChapterNode, Guid>(builder);

        builder.Property(x => x.PropertyValueID)
               .HasColumnType("varchar")
               .HasMaxLength(50)
               .HasComment("从题库迁移的数据");

        builder.Property(x => x.Code)
               .HasColumnType("varchar")
               .HasMaxLength(50)
               .HasComment("编码");

        builder.Property(x => x.Name)
               .HasColumnType("varchar")
               .HasMaxLength(50)
               .HasComment("章节名称");

        builder.Property(x => x.ParentName)
               .HasColumnType("varchar")
               .HasMaxLength(50)
               .HasComment("父级名称");

        builder.Property(x => x.ParentIds)
               .HasColumnType("longtext").HasConversion(listConverter)
               .HasComment("父级Id集合");

        builder.HasMany(x => x.Children)
               .WithOne()
               .HasForeignKey(x => x.ParentId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.KnowledgePoints)
               .WithOne()
               .HasForeignKey(x => x.ChapterNodeId)
               .OnDelete(DeleteBehavior.Cascade);

        //索引
        builder.HasIndex(x => new { x.SubjectId, x.ParentId, x.TenantId });
    }
}