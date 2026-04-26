using HaoKao.FeedBackService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HaoKao.FeedBackService.Infrastructure.EntityConfigurations;

public class SuggestionEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<Suggestion>
{
    public override void Configure(EntityTypeBuilder<Suggestion> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<Suggestion, Guid>(builder);

        var jsonConverter = new ValueConverter<List<string>, string>(
            v => JsonConvert.SerializeObject(v),
            v => string.IsNullOrEmpty(v)
                ? Array.Empty<string>().ToList()
                : JsonConvert.DeserializeObject<List<string>>(v)
        );

        builder.Property(x => x.SuggestionType).HasComment("反馈类型");
        builder.Property(x => x.SuggestionFrom).HasColumnType("varchar").HasMaxLength(50).HasComment("反馈来源");
        builder.Property(x => x.Phone).HasColumnType("varchar").HasMaxLength(50).HasComment("手机号");
        builder.Property(x => x.Description).HasColumnType("varchar").HasMaxLength(500).HasComment("问题描述");
        builder.Property(x => x.Screenshots).HasColumnType("json").HasComment("相关截图").HasConversion(jsonConverter);
        builder.Property(x => x.CreatorId).HasComment("反馈人Id");
        builder.Property(x => x.CreatorName).HasColumnType("varchar").HasMaxLength(50).HasComment("反馈人名称");
        builder.Property(x => x.CreateTime).HasColumnType("varchar").HasMaxLength(50).HasComment("反馈时间");
        builder.Property(x => x.ReplyState).HasComment("处理状态");
        builder.Property(x => x.ReplyUserId).HasComment("处理人Id");
        builder.Property(x => x.ReplyUserName).HasColumnType("varchar").HasMaxLength(50).HasComment("处理人名称");
        builder.Property(x => x.ReplyTime).HasComment("回复时间");
        builder.Property(x => x.ReplyContent).HasColumnType("varchar").HasMaxLength(500).HasComment("回复内容");
        builder.Property(x => x.ReplyScreenshots).HasColumnType("json").HasComment("回复截图").HasConversion(jsonConverter);
        builder.Property(x => x.TenantId).HasComment("租户Id");

        builder.HasIndex(x => new { x.Phone, x.CreatorId, x.TenantId });
    }
}