using HaoKao.NoticeService.Domain.Commands;

namespace HaoKao.NoticeService.Application.ViewModels;

[AutoMapTo(typeof(UpdateNoticeCommand))]
public class UpdateNoticeViewModel : CreateNoticeViewModel
{
    /// <summary>
    /// 公告Id
    /// </summary>
    [DisplayName("公告Id")]
    public Guid Id { get; set; }
}

[AutoMapTo(typeof(UpdateNoticePopupCommand))]
public class UpdateNoticePopupViewModel
{
    /// <summary>
    /// 公告Ids
    /// </summary>
    [DisplayName("公告Ids")]
    public List<Guid> Ids { get; set; }

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

[AutoMapTo(typeof(UpdateNoticePublishedCommand))]
public class UpdateNoticePublishedViewModel
{
    /// <summary>
    /// 公告Ids
    /// </summary>
    [DisplayName("公告Ids")]
    public List<Guid> Ids { get; set; }

    /// <summary>
    /// 是否发布
    /// </summary>
    [DisplayName("是否发布")]
    public bool Published { get; set; }
}