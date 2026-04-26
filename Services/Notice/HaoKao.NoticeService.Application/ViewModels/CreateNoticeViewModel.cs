using HaoKao.NoticeService.Domain.Commands;

namespace HaoKao.NoticeService.Application.ViewModels;

[AutoMapTo(typeof(CreateNoticeCommand))]
public class CreateNoticeViewModel : IDto
{
    /// <summary>
    /// 公告名称
    /// </summary>
    [DisplayName("公告名称")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MaxLength(50, ErrorMessage = "{0}长度不能大于{1}")]
    public string Title { get; set; }

    /// <summary>
    /// 公告内容
    /// </summary>
    [DisplayName("公告内容")]
    [Required(ErrorMessage = "{0}不能为空")]
    public string Content { get; set; }

    /// <summary>
    /// 是否发布
    /// </summary>
    [DisplayName("是否发布")]
    public bool Published { get; set; }

    /// <summary>
    /// 是否弹出
    /// </summary>
    [DisplayName("是否弹出")]
    public bool Popup { get; set; }

    /// <summary>
    /// 弹出开始时间
    /// </summary>
    [DisplayName("弹出开始时间")]
    public DateTime? StartTime { get; set; }

    /// <summary>
    /// 弹出结束时间
    /// </summary>
    [DisplayName("弹出结束时间")]
    public DateTime? EndTime { get; set; }
}