using HaoKao.GroupBookingService.Application.ViewModels.GroupData;

namespace HaoKao.GroupBookingService.Application.Services.Web;

public interface IGroupDataWebService : IAppWebApiService, IManager
{
    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    Task<GroupDataQueryViewModel> Get([FromQuery] GroupDataQueryViewModel queryViewModel);
}