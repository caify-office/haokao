namespace HaoKao.LiveBroadcastService.Application.ViewModels.LiveVideo;

public class CreateLiveVideoUrlViewModel : IDto
{
    /// <summary>
    /// app名称
    /// </summary>
    [DisplayName("app名称")]
    [Required(ErrorMessage = "{0}不能为空")]
    public string appName { get; set; }

    /// <summary>
    /// 流名称
    /// </summary>

    [DisplayName("流名称")]
    [Required(ErrorMessage = "{0}不能为空")]
    public string streamName { get; set; }

    /// <summary>
    /// 过期时间（单位是秒)
    /// </summary>
    [DisplayName("过期时间（单位是秒)")]
    [Required(ErrorMessage = "{0}不能为空")]
    public long expireTime { get; set; }

    /// <summary>
    /// 直播Id
    /// </summary>
    [DisplayName("直播Id")]
    [Required(ErrorMessage = "{0}不能为空")]
    public Guid LiveVideoId { get; set; }
}