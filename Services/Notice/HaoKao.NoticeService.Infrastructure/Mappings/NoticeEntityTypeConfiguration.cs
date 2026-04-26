using HaoKao.NoticeService.Domain.Models;

namespace HaoKao.NoticeService.Infrastructure.Mappings;

public class NoticeEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<Notice>
{
    public override void Configure(EntityTypeBuilder<Notice> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<Notice, Guid>(builder);

        builder.Property(x => x.Title).IsRequired().HasComment("公告名称");
        builder.Property(x => x.Content).HasColumnType("longtext").IsRequired().HasComment("公告内容");
        builder.Property(x => x.Popup).HasComment("是否弹出");
        builder.Property(x => x.Published).HasComment("是否发布");
        builder.Property(x => x.StartTime).HasComment("弹出开始时间");
        builder.Property(x => x.EndTime).HasComment("弹出结束时间");
        builder.Property(x => x.CreateTime).HasComment("创建时间");
        builder.Property(x => x.CreatorId).HasComment("创建人Id");
        builder.Property(x => x.CreatorName).HasColumnType("varchar(40)").HasComment("创建人名称");
        builder.Property(x => x.TenantId).HasComment("租户Id");
     
    }
}