using HaoKao.ProductService.Domain.Entities;

namespace HaoKao.ProductService.Infrastructure.Mappings;

public class SupervisorStudentEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<SupervisorStudent>
{
    public override void Configure(EntityTypeBuilder<SupervisorStudent> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<SupervisorStudent, Guid>(builder);

        builder.Property(x => x.Name)
               .HasColumnType("varchar")
               .HasMaxLength(50)
               .HasComment("昵称");

        builder.Property(x => x.Phone)
               .HasColumnType("varchar")
               .HasMaxLength(50)
               .HasComment("手机号");

        builder.Property(x => x.LastLearnTime)
               .HasColumnType("datetime")
               .HasComment("最近学习时间");

        builder.Property(x => x.StatisticsTime)
               .HasColumnType("datetime")
               .HasComment("统计时间 （每一天只统计一次）");
    }
}