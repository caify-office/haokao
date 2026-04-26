using HaoKao.LiveBroadcastService.Domain.Commands.LiveComment;

namespace HaoKao.LiveBroadcastService.Application.ViewModels.LiveComment;

[AutoMapTo(typeof(CreateLiveCommentCommand))]
public class CreateLiveCommentViewModel : IDto
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
    /// 评分
    /// </summary>
    [DisplayName("评分")]
    [Required(ErrorMessage = "{0}不能为空")]
    public int Rating { get; set; }

    /// <summary>
    /// 评价内容
    /// </summary>
    [DisplayName("评价内容")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MinLength(2, ErrorMessage = "{0}长度不能小于{1}")]
    [MaxLength(300, ErrorMessage = "{0}长度不能大于{1}")]
    public string Content { get; set; }

    /// <summary>
    /// 直播Id
    /// </summary>
    [DisplayName("直播Id")]
    [Required(ErrorMessage = "{0}不能为空")]
    public Guid LiveId { get; set; }
}