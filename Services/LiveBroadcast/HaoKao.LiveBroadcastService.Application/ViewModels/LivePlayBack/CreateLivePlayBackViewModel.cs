using HaoKao.LiveBroadcastService.Domain.Commands.LivePlayBack;
using HaoKao.LiveBroadcastService.Domain.Enums;

namespace HaoKao.LiveBroadcastService.Application.ViewModels.LivePlayBack;

[AutoMapTo(typeof(CreateLivePlayBackModel))]
public class CreateLivePlayBackViewModel : IDto
{
    /// <summary>
    /// 所属视频直播Id
    /// </summary>
    [DisplayName("所属视频直播Id")]
    [Required(ErrorMessage = "{0}不能为空")]
    public Guid LiveVideoId { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    [DisplayName("名称")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MinLength(2, ErrorMessage = "{0}长度不能小于{1}")]
    [MaxLength(500, ErrorMessage = "{0}长度不能大于{1}")]
    public string Name { get; set; }

    /// <summary>
    /// 视频格式
    /// </summary>
    [DisplayName("视频格式")]
    [Required(ErrorMessage = "{0}不能为空")]
    public VideoType VideoType { get; set; }

    /// <summary>
    /// 视频时长
    /// </summary>
    [DisplayName("视频时长")]
    [Required(ErrorMessage = "{0}不能为空")]
    public decimal Duration { get; set; }

    /// <summary>
    /// 视频序号
    /// </summary>
    [DisplayName("视频序号")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MinLength(2, ErrorMessage = "{0}长度不能小于{1}")]
    [MaxLength(50, ErrorMessage = "{0}长度不能大于{1}")]
    public string VideoNo { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    [DisplayName("排序")]
    [Required(ErrorMessage = "{0}不能为空")]
    public int Sort { get; set; }
}