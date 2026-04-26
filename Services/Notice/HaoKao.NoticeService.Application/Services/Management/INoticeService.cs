using HaoKao.NoticeService.Application.ViewModels;

namespace HaoKao.NoticeService.Application.Services.Management;

public interface INoticeService : IAppWebApiService, IManager
{
    /// <summary>
    /// 根据主键获取指定公共
    /// </summary>
    /// <param name="id">主键</param>
    Task<BrowseNoticeViewModel> Get(Guid id);

    /// <summary>
    /// 根据查询获取公告列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    Task<QueryNoticeViewModel> Get(QueryNoticeViewModel queryViewModel);

    /// <summary>
    /// 创建公告
    /// </summary>
    /// <param name="model">新增模型</param>
    Task Create(CreateNoticeViewModel model);

    /// <summary>
    /// 删除公告
    /// </summary>
    /// <param name="ids">主键</param>
    Task Delete(List<Guid> ids);

    /// <summary>
    /// 更新公告
    /// </summary>
    /// <param name="model">更新模型</param>
    Task Update(UpdateNoticeViewModel model);

    /// <summary>
    /// 修改公告是否弹出
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task UpdatePopup(UpdateNoticePopupViewModel model);

    /// <summary>
    /// 修改公告是否发布
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task UpdatePublished(UpdateNoticePublishedViewModel model);
}