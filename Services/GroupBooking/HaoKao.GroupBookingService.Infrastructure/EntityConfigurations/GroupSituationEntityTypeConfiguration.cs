using HaoKao.GroupBookingService.Domain.Entities;
using System;

namespace HaoKao.GroupBookingService.Infrastructure.EntityConfigurations;

public class GroupSituationEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<GroupSituation>
{
    public override void Configure(EntityTypeBuilder<GroupSituation> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<GroupSituation, Guid>(builder);

      

        builder.Property(x => x.GroupDataId)
               .IsRequired()
               .HasComment("组团资料Id");

        builder.Property(x => x.DataName)
            .IsRequired()
            .HasColumnType("varchar")
            .HasMaxLength(50)
            .HasComment("资料名");

        builder.Property(x => x.SuitableSubjects)
            .HasColumnType("json")
            .HasComment("适用科目");

        builder.Property(x => x.Document)
            .HasColumnType("text")
            .HasComment("拼团资料");

        builder.Property(x => x.PeopleNumber).IsRequired().HasComment("成团人数");
        builder.Property(x => x.LimitTime).IsRequired().HasComment("限制时间");

        builder.Property(x => x.SuccessTime).HasComment("组团成功时间");

        builder.HasIndex(x => new { x.GroupDataId,x.SuccessTime, x.DataName, x.TenantId });
    }
}
