using HaoKao.BurialPointService.Application.ViewModels.BrowseRecord;

namespace HaoKao.BurialPointService.Application.Services.Management;

public interface IBrowseRecordService : IAppWebApiService, IManager
{
    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    Task<BrowseBrowseRecordViewModel> Get(Guid id);

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    Task<BrowseRecordQueryViewModel> Get(BrowseRecordQueryViewModel queryViewModel);

    /// <summary>
    /// 创建浏览记录
    /// </summary>
    /// <param name="model">新增模型</param>
    Task Create(CreateBrowseRecordViewModel model);

}