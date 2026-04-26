using HaoKao.NoticeService.Domain.Queries;

namespace HaoKao.NoticeService.Application.ViewModels;

[AutoMapFrom(typeof(NoticeQuery))]
[AutoMapTo(typeof(NoticeQuery))]
public class QueryNoticeViewModel : QueryDtoBase<BrowseNoticeViewModel>
{
    /// <summary>
    /// 公告名称
    /// </summary>
    public string Title { get; set; }
}