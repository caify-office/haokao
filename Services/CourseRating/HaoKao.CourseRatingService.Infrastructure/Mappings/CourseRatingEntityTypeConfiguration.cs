using HaoKao.CourseRatingService.Domain.Entities;

namespace HaoKao.CourseRatingService.Infrastructure.Mappings;

public class CourseRatingEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<CourseRating>
{
    public override void Configure(EntityTypeBuilder<CourseRating> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<CourseRating, Guid>(builder);

        builder.Property(x => x.CourseName)
               .HasColumnType("varchar")
               .HasMaxLength(50)
               .IsRequired()
               .HasComment("课程名称");

        builder.Property(x => x.Comment)
               .HasColumnType("varchar")
               .HasMaxLength(300)
               .IsRequired()
               .HasComment("评价内容");

        builder.Property(x => x.Avatar)
           .HasColumnType("varchar")
           .HasMaxLength(300)
           .IsRequired()
           .HasComment("头像");

        builder.Property(x => x.NickName)
               .HasColumnType("varchar")
               .HasMaxLength(50)
               .IsRequired()
               .HasComment("昵称");

        builder.Property(x => x.AuditState).HasComment("审核状态");
        builder.Property(x => x.Sticky).HasComment("是否置顶");
        builder.Property(x => x.Rating).HasComment("评价级别");
        builder.Property(x => x.CourseId).HasComment("课程Id");
        builder.Property(x => x.TenantId).HasComment("租户Id");
        builder.Property(x => x.CreateTime).HasComment("评价时间");
        builder.Property(x => x.UpdateTime).HasComment("修改时间");
        builder.Property(x => x.CreatorId).HasComment("评价人");
        builder.Property(x => x.Avatar).HasComment("头像");
        builder.HasIndex(x => new { x.CourseId, x.CreatorId, x.TenantId, });
    }
}