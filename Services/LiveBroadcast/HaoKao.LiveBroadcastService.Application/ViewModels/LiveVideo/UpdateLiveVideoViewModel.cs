using HaoKao.LiveBroadcastService.Domain.Enums;

namespace HaoKao.LiveBroadcastService.Application.ViewModels.LiveVideo;

[AutoMapTo(typeof(UpdateLiveVideoCommand))]
public class UpdateLiveVideoViewModel : IDto
{
    public Guid Id { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    [DisplayName("名称")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MinLength(2, ErrorMessage = "{0}长度不能小于{1}")]
    [MaxLength(50, ErrorMessage = "{0}长度不能大于{1}")]
    public string Name { get; set; }

    /// <summary>
    /// 卡片Url
    /// </summary>

    [DisplayName("卡片")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MinLength(2, ErrorMessage = "{0}长度不能小于{1}")]
    [MaxLength(300, ErrorMessage = "{0}长度不能大于{1}")]
    public string CardUrl { get; set; }

    /// <summary>
    /// 直播类型
    /// </summary>
    [DisplayName("直播类型")]
    [Required(ErrorMessage = "{0}不能为空")]
    public LiveType LiveType { get; set; }

    /// <summary>
    /// 科目Id
    /// </summary>
    [DisplayName("科目Id")]
    public List<Guid> SubjectIds { get; set; }

    /// <summary>
    /// 科目名称
    /// </summary>
    [DisplayName("科目名称")]
    public List<string> SubjectNames { get; set; }

    /// <summary>
    /// 主讲老师Id
    /// </summary>
    [DisplayName("主讲老师Id")]
    [Required(ErrorMessage = "{0}不能为空")]
    public List<Guid> LecturerId { get; set; }

    /// <summary>
    /// 主讲老师名称
    /// </summary>
    [DisplayName("主讲老师名称")]
    [Required(ErrorMessage = "{0}不能为空")]
    public List<string> LecturerName { get; set; }

    /// <summary>
    /// 直播开始时间
    /// </summary>
    [DisplayName("直播开始时间")]
    [Required(ErrorMessage = "{0}不能为空")]
    public DateTime StartTime { get; set; }

    /// <summary>
    /// 直播结束时间
    /// </summary>
    [DisplayName("直播结束时间")]
    [Required(ErrorMessage = "{0}不能为空")]
    public DateTime EndTime { get; set; }

    /// <summary>
    /// 直播状态
    /// </summary>
    [DisplayName("直播状态")]
    [Required(ErrorMessage = "{0}不能为空")]
    public LiveStatus LiveStatus { get; set; }

    /// <summary>
    /// 购买产品跳转对象Id
    /// </summary>
    [DisplayName("购买产品跳转对象Id")]
    public Guid TargetProductId { get; set; }

    /// <summary>
    /// 详情介绍
    /// </summary>
    [DisplayName("详情介绍")]
    public string Desc { get; set; }

    /// <summary>
    /// 播流地址
    /// </summary>
    [DisplayName("播流地址")]
    public Dictionary<string, string> LiveAddress { get; set; }

    /// <summary>
    /// 推流地址
    /// </summary>
    [DisplayName("推流地址")]
    public string StreamingAddress { get; set; }

    /// <summary>
    /// 直播公告Id
    /// </summary>
    [DisplayName("直播公告Id")]
    public Guid? LiveAnnouncementId { get; set; }
}