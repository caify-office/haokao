using HaoKao.LiveBroadcastService.Domain.Commands.LiveMessage;
using HaoKao.LiveBroadcastService.Domain.Enums;

namespace HaoKao.LiveBroadcastService.Application.ViewModels.LiveMessage;

[AutoMapTo(typeof(CreateLiveMessageCommand))]
public class CreateLiveMessageViewModel : IDto
{
    /// <summary>
    /// 消息Id
    /// </summary>
    [DisplayName("消息Id")]
    public Guid Id { get; set; }

    /// <summary>
    /// 直播间Id
    /// </summary>
    [DisplayName("直播间Id")]
    public Guid LiveId { get; set; }

    /// <summary>
    /// 消息内容
    /// </summary>
    [DisplayName("消息内容")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MaxLength(500, ErrorMessage = "{0}长度不能大于{1}")]
    public string Content { get; set; }

    /// <summary>
    /// 消息类型
    /// </summary>
    [DisplayName("消息类型")]
    public LiveMessageType LiveMessageType { get; set; }
}