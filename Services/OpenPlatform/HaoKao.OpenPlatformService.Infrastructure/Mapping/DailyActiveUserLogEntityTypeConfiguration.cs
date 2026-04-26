namespace HaoKao.OpenPlatformService.Infrastructure.Mapping;

public class DailyActiveUserLogEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<DailyActiveUserLog>
{
    public override void Configure(EntityTypeBuilder<DailyActiveUserLog> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<DailyActiveUserLog, Guid>(builder);

        builder.Property(x => x.UserId).HasComment("用户Id");
        builder.Property(x => x.ClientId).HasColumnType("varchar(50)").IsRequired().HasComment("活跃的客户端Id");
        builder.Property(x => x.CreateDate).HasColumnType("varchar(20)").IsRequired().HasComment("活跃的日期(yyyy-MM-dd)");
        builder.Property(x => x.CreateTime).HasComment("创建时间");

        builder.HasIndex(x => new { x.UserId, x.ClientId, x.CreateDate, x.CreateTime });
    }
}