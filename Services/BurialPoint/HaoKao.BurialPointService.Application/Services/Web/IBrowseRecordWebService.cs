using HaoKao.BurialPointService.Application.ViewModels.BrowseRecord;

namespace HaoKao.BurialPointService.Application.Services.Web;

public interface IBrowseRecordWebService : IAppWebApiService, IManager
{
    /// <summary>
    /// 创建浏览记录
    /// </summary>
    /// <param name="model">新增模型</param>
    Task Create(CreateBrowseRecordViewModel model);

}