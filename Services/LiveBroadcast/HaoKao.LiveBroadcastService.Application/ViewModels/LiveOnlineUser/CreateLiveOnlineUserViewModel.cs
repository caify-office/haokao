using HaoKao.LiveBroadcastService.Domain.Commands.LiveOnlineUser;

namespace HaoKao.LiveBroadcastService.Application.ViewModels.LiveOnlineUser;

[AutoMapTo(typeof(CreateLiveOnlineUserCommand))]
public class CreateLiveOnlineUserViewModel : IDto
{
    /// <summary>
    /// Id
    /// </summary>
    [DisplayName("Id")]
    public Guid Id { get; set; }

    /// <summary>
    /// 手机号
    /// </summary>
    [DisplayName("手机号")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MinLength(2, ErrorMessage = "{0}长度不能小于{1}")]
    [MaxLength(50, ErrorMessage = "{0}长度不能大于{1}")]
    public string Phone { get; set; }

    /// <summary>
    /// 直播Id
    /// </summary>
    [DisplayName("直播Id")]
    [Required(ErrorMessage = "{0}不能为空")]
    public Guid LiveId { get; set; }
}