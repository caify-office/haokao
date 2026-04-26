using HaoKao.LiveBroadcastService.Domain.Commands.LiveOnlineUser;

namespace HaoKao.LiveBroadcastService.Application.ViewModels.LiveOnlineUser;

[AutoMapTo(typeof(UpdateLiveOnlineUserCommand))]
public class UpdateLiveOnlineUserViewModel : IDto
{
    /// <summary>
    /// Id
    /// </summary>
    [DisplayName("Id")]
    [Required(ErrorMessage = "{0}不能为空")]
    public Guid Id { get; set; }

    /// <summary>
    /// 是否在线
    /// </summary>
    [DisplayName("是否在线")]
    [Required(ErrorMessage = "{0}不能为空")]
    public bool IsOnline { get; set; }

    /// <summary>
    /// 用户昵称
    /// </summary>
    [DisplayName("用户昵称")]
    [Required(ErrorMessage = "{0}不能为空")]
    public string CreatorName { get; set; }
}