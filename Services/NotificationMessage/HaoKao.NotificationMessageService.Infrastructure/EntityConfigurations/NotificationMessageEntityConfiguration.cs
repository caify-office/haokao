namespace HaoKao.NotificationMessageService.Infrastructure.EntityConfigurations;

public class NotificationMessageEntityConfiguration : GirvsAbstractEntityTypeConfiguration<NotificationMessage>
{
    public override void Configure(
        EntityTypeBuilder<NotificationMessage> builder)
    {
        OnModelCreatingBaseEntityAndTableKey<NotificationMessage, Guid>(
            builder);
        builder.Property(x => x.Title).HasColumnType("varchar(100)").HasComment("消息标题");
        builder.Property(x => x.ParameterContent).HasColumnType("varchar(500)").HasComment("消息内容");
        builder.Property(x => x.CreatorName).HasColumnType("varchar(50)").HasComment("创建者");
        builder.Property(x => x.Failure).HasColumnType("varchar(255)").HasComment("发送失败内容");
        builder.Property(x => x.Receiver).HasColumnType("varchar(50)").HasComment("接收者");
        builder.Property(x => x.MessageTemplateId).HasColumnType("varchar(100)").HasComment("消息模板Id");
        builder.Property(x => x.IdCard).HasColumnType("varchar(18)").HasComment("身份证号码");


        builder.HasIndex(x => new
        {
            x.CreateTime,
            x.Receiver,
            x.ParameterContent,
            x.Title,
            x.IdCard,
            x.NotificationMessageType,
            x.ReceivingChannel,
            x.SendState,
            x.IsRead
        });
    }
}