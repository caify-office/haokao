using HaoKao.LiveBroadcastService.Domain.Commands.MutedUser;

namespace HaoKao.LiveBroadcastService.Application.ViewModels.MutedUser;

[AutoMapTo(typeof(CreateMutedUserCommand))]
public class CreateMutedUserViewModel : IDto
{
    /// <summary>
    /// 手机号
    /// </summary>
    [DisplayName("手机号")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MinLength(2, ErrorMessage = "{0}长度不能小于{1}")]
    [MaxLength(50, ErrorMessage = "{0}长度不能大于{1}")]
    public string Phone { get; set; }

    /// <summary>
    /// 用户Id
    /// </summary>
    [DisplayName("用户Id")]
    [Required(ErrorMessage = "{0}不能为空")]
    public Guid UserId { get; set; }

    /// <summary>
    /// 用户名称
    /// </summary>
    [DisplayName("用户名称")]
    [Required(ErrorMessage = "{0}不能为空")]
    public string UserName { get; set; }
}