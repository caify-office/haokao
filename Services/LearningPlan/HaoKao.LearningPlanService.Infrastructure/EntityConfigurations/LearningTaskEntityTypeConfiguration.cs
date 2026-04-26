using HaoKao.LearningPlanService.Domain.Records;

namespace HaoKao.LearningPlanService.Infrastructure.EntityConfigurations;

public class LearningTaskEntityTypeConfiguration : GirvsAbstractEntityTypeConfiguration<LearningTask>
{
    public override void Configure(EntityTypeBuilder<LearningTask> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<LearningTask, Guid>(builder);

        builder.Property(x => x.TaskName)
            .HasColumnType("varchar")
            .HasMaxLength(500)
            .HasComment("任务名称");

        builder.Property(x => x.ScheduledTime)
            .HasColumnType("date")
            .HasComment("计划开始该任务的时间点");

        builder.Property(x => x.DurationSeconds)
            .HasColumnType("decimal(7,2)")
            .HasMaxLength(10)
            .HasComment("完成该任务预计需要的时长（秒），支持小数表示");

        builder.Property(x => x.TaskIdentity)
            .HasColumnType("varchar")
            .HasMaxLength(50)
            .HasComment("任务标识（用于重新制作学习计划时判定当前任务内容是否已是过期任务内容）");


       // var taskContentConverter = new ValueConverter<object, string>(
       //    v => JsonConvert.SerializeObject(v),
       //    v => v.IsNullOrEmpty() ? (object)new object() : JsonConvert.DeserializeObject<object>(v)
       //);

        builder.Property(x => x.TaskContent)
            .HasColumnType("json")
            //.HasConversion(taskContentConverter)
            .HasComment("任务内容");

        builder.HasIndex(x => new { x.ScheduledTime, x.LearningPlanId });
    }
}