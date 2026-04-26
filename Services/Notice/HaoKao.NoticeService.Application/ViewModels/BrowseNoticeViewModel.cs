using HaoKao.NoticeService.Domain.Models;

namespace HaoKao.NoticeService.Application.ViewModels;

[AutoMapFrom(typeof(Notice))]
public class BrowseNoticeViewModel : IDto
{
    /// <summary>
    /// 主键
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// 公告名称
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// 公告内容
    /// </summary>
    public string Content { get; set; }

    /// <summary>
    /// 是否发布
    /// </summary>
    public bool Published { get; set; }

    /// <summary>
    /// 是否弹出
    /// </summary>
    public bool Popup { get; set; }

    /// <summary>
    /// 弹出开始时间
    /// </summary>
    public DateTime? StartTime { get; set; }

    /// <summary>
    /// 弹出结束时间
    /// </summary>
    public DateTime? EndTime { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 创建人名称
    /// </summary>
    public string CreatorName { get; set; }

    /// <summary>
    /// 创建人Id
    /// </summary>
    public Guid CreatorId { get; set; }
}