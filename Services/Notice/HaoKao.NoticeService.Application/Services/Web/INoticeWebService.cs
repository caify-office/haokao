using HaoKao.NoticeService.Application.ViewModels;

namespace HaoKao.NoticeService.Application.Services.Web;

public interface INoticeWebService : IAppWebApiService, IManager
{
    /// <summary>
    /// 获取公告列表
    /// </summary>
    /// <param name="popup">是否弹出</param>
    /// <returns></returns>
    Task<List<BrowseNoticeViewModel>> Get(bool? popup);
}