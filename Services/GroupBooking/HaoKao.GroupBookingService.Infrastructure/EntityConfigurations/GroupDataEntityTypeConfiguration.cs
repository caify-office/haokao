using HaoKao.GroupBookingService.Domain.Entities;
using System;

namespace HaoKao.GroupBookingService.Infrastructure.EntityConfigurations;

public class GroupDataEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<GroupData>
{
    public override void Configure(EntityTypeBuilder<GroupData> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<GroupData, Guid>(builder);

 

        builder.Property(x => x.DataName)
            .IsRequired()
            .HasColumnType("varchar")
            .HasMaxLength(50)
            .HasComment("资料名");

        builder.Property(x => x.SuitableSubjects)
            .HasColumnType("json")
            .HasComment("适用科目");

        builder.Property(x => x.PeopleNumber).IsRequired().HasComment("成团人数");
        builder.Property(x => x.BasePeopleNumber).IsRequired().HasComment("基础拼团成功人数");
        builder.Property(x => x.LimitTime).IsRequired().HasComment("限制时间");

        builder.Property(x => x.Document)
            .HasColumnType("text")
            .HasComment("拼团资料");

        builder.Property(x => x.State).HasColumnType("bit").HasComment("状态");

    }
}