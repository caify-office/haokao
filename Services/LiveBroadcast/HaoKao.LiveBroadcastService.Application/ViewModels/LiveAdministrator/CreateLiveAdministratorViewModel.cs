using HaoKao.LiveBroadcastService.Domain.Commands.LiveAdministrator;

namespace HaoKao.LiveBroadcastService.Application.ViewModels.LiveAdministrator;

[AutoMapTo(typeof(CreateLiveAdministratorCommand))]
public class CreateLiveAdministratorViewModel : IDto
{
    /// <summary>
    /// 用户Id
    /// </summary>
    [DisplayName("用户Id")]
    [Required(ErrorMessage = "{0}不能为空")]
    public Guid UserId { get; set; }

    /// <summary>
    /// 姓名
    /// </summary>
    [DisplayName("姓名")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MaxLength(50, ErrorMessage = "{0}长度不能大于{1}")]
    public string Name { get; set; }

    /// <summary>
    /// 手机号
    /// </summary>
    [DisplayName("手机号")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MinLength(2, ErrorMessage = "{0}长度不能小于{1}")]
    [MaxLength(50, ErrorMessage = "{0}长度不能大于{1}")]
    public string Phone { get; set; }
}