using HaoKao.KnowledgePointService.Domain.Entities;
using System;

namespace HaoKao.KnowledgePointService.Infrastructure.Mapping;

public class KnowledgePointEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<KnowledgePoint>
{
    public override void Configure(EntityTypeBuilder<KnowledgePoint> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<KnowledgePoint, Guid>(builder);

        builder.Property(x => x.Name).IsRequired().HasColumnType("varchar(100)").HasComment("知识点名称");
        builder.Property(x => x.ChapterNodeId).IsRequired().HasColumnType("varchar(36)").HasComment("章节Id");
        builder.Property(x => x.ChapterNodeName).IsRequired().HasColumnType("varchar(100)").HasComment("章节名称");

        builder.HasIndex(x => new { ChpaterNodeId = x.ChapterNodeId, x.Name, x.TenantId });
    }
}