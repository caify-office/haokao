using Girvs.Extensions;
using HaoKao.LiveBroadcastService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;

namespace HaoKao.LiveBroadcastService.Infrastructure.EntityConfigurations;

public class LiveVideoEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<LiveVideo>
{
    public override void Configure(EntityTypeBuilder<LiveVideo> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<LiveVideo, Guid>(builder);

        builder.Property(x => x.Name)
               .HasColumnType("varchar")
               .HasMaxLength(50)
               .HasComment("名称");

        builder.Property(x => x.CardUrl)
               .HasColumnType("varchar")
               .HasMaxLength(300)
               .HasComment("卡片");

        var lecturerIdConverter = new ValueConverter<List<Guid>, string>(
            v => JsonConvert.SerializeObject(v),
            v => v.IsNullOrEmpty() ? new List<Guid>() : JsonConvert.DeserializeObject<List<Guid>>(v)
        );
        builder.Property(x => x.SubjectIds)
               .HasColumnType("text")
               .HasConversion(lecturerIdConverter)
               .HasComment("科目Id");

        builder.Property(x => x.SubjectIdsStr)
               .HasColumnType("text")
               .HasComment("科目Id字符串");

        builder.Property(x => x.LecturerId)
               .HasColumnType("text")
               .HasConversion(lecturerIdConverter)
               .HasComment("主讲老师Id");

        var lecturerNameConverter = new ValueConverter<List<string>, string>(
            v => JsonConvert.SerializeObject(v),
            v => v.IsNullOrEmpty() ? new List<string>() : JsonConvert.DeserializeObject<List<string>>(v)
        );
        builder.Property(x => x.SubjectNames)
               .HasColumnType("text")
               .HasConversion(lecturerNameConverter)
               .HasComment("科目名称");

        builder.Property(x => x.LecturerName)
               .HasColumnType("text")
               .HasConversion(lecturerNameConverter)
               .HasComment("主讲老师名称");

        builder.Property(x => x.StartTime)
               .HasColumnType("datetime")
               .HasComment("直播开始时间");

        builder.Property(x => x.EndTime)
               .HasColumnType("datetime")
               .HasComment("直播结束时间");

        var liveProductPackagesConverter = new ValueConverter<List<LiveProductPackage>, string>(
            v => JsonConvert.SerializeObject(v),
            v => v.IsNullOrEmpty() ? new List<LiveProductPackage>() : JsonConvert.DeserializeObject<List<LiveProductPackage>>(v)
        );


        var livePlayBacksConverter = new ValueConverter<List<LivePlayBack>, string>(
            v => JsonConvert.SerializeObject(v),
            v => v.IsNullOrEmpty() ? new List<LivePlayBack>() : JsonConvert.DeserializeObject<List<LivePlayBack>>(v)
        );


        builder.Property(x => x.Desc)
               .HasColumnType("text")
               .HasComment("详情介绍");

        var dictionaryConverter = new ValueConverter<Dictionary<string, string>, string>(
           v => JsonConvert.SerializeObject(v),
           v => JsonConvert.DeserializeObject<Dictionary<string, string>>(v));

        builder.Property(x => x.LiveAddress)
               .HasColumnType("json")
               .HasConversion(dictionaryConverter)
               .HasComment("播流地址");

        builder.Property(x => x.StreamingAddress)
               .HasColumnType("varchar")
               .HasMaxLength(300)
               .HasComment("推流地址");

        builder.HasMany(x => x.LiveCoupons)
               .WithOne(x => x.LiveVideo)
               .HasForeignKey(x => x.LiveVideoId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => new { x.LiveStatus,x.LiveType, x.Name, });
    }
}