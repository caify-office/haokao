namespace HaoKao.NotificationMessageService.Application.ViewModels.NotificationMessage;

/// <summary>
/// 我的消息列表查询条件
/// </summary>
[AutoMapFrom(typeof(NotificationMessageQuery))]
[AutoMapTo(typeof(NotificationMessageQuery))]
public class ExamineeNotificationMessageQueryViewModel : QueryDtoBase<SiteNotificationMessageQueryListViewModel>
{
    public ExamineeNotificationMessageQueryViewModel()
    {
        OrderBy = nameof(Domain.Models.NotificationMessage.CreateTime);
    }
    /// <summary>
    /// 消息所属者的身份证号码
    /// </summary>
    [DisplayName("消息所属者的身份证号码")]
    [Required(ErrorMessage = "{0}不能为空")]
    public string IdCard { get; set; }
    /// <summary>
    /// 接收渠道
    /// </summary>
    public ReceivingChannel? ReceivingChannel { get; set; }
        
    /// <summary>
    /// 对应的访问Id
    /// </summary>
    public Guid? TenantAccessId { get; set; }
}

[AutoMapFrom(typeof(Domain.Models.NotificationMessage))]
[AutoMapTo(typeof(Domain.Models.NotificationMessage))]
public class SiteNotificationMessageQueryListViewModel : IDto
{
    public Guid Id { get; set; }
    /// <summary>
    /// 消息标题
    /// </summary>
    [JsonIgnore]
    public string Title { get; set; }

    /// <summary>
    /// 参数消息内容
    /// </summary>
    [JsonIgnore]
    public string ParameterContent { get; set; }
    /// <summary>
    /// 消息内容
    /// </summary>
    public string Content 
    {
        get
        {
            try
            {
                var parms = Title.Split(',').ToList();
                return string.Format(ParameterContent, parms.ToArray());
            }
            catch 
            {
                return string.Empty;
            }
               
        }
    }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 是否阅读
    /// </summary>
    public bool IsRead { get; set; }

    public Guid TenantId { get; set; }
}