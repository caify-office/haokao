namespace HaoKao.LearningPlanService.Infrastructure.EntityConfigurations;

public class LearningPlanEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<LearningPlan>
{
    public override void Configure(EntityTypeBuilder<LearningPlan> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<LearningPlan, Guid>(builder);

        builder.Property(x => x.SubjectName)
            .HasColumnType("varchar")
            .HasMaxLength(50)
            .HasComment("对应科目名称");

        builder.Property(x => x.ReminderPhone)
           .HasColumnType("varchar")
           .HasMaxLength(50)
           .HasComment("提醒手机号");


        builder.Property(x => x.EndDate)
            .HasColumnType("date")
            .HasComment("学习计划结束日期");

        builder.HasMany(x => x.LearningTasks)
               .WithOne(x => x.LearningPlan)
               .HasForeignKey(x => x.LearningPlanId)
               .OnDelete(DeleteBehavior.Cascade);

       

        var dayLearningTimesConverter = new ValueConverter<ICollection<int>, string>(
            v => JsonConvert.SerializeObject(v),
            v => v.IsNullOrEmpty() ? new List<int>() : JsonConvert.DeserializeObject<List<int>>(v)
        );

        builder.Property(x => x.DayLearningTimes)
            .HasColumnType("json")
            .HasConversion(dayLearningTimesConverter)
            .HasComment("每周学习时长配置(分钟)");

    }
}