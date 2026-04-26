using HaoKao.ErrorCorrectingService.Application.ViewModels.ErrorCorrecting;

namespace HaoKao.ErrorCorrectingService.Application.AppService.Management;

public interface IErrorCorrectingService : IAppWebApiService, IManager
{
    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    Task<BrowseErrorCorrectingViewModel> Get(Guid id);

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    Task<ErrorCorrectingQueryViewModel> Get(ErrorCorrectingQueryViewModel queryViewModel);

    /// <summary>
    /// 根据主键删除指定本题纠错
    /// </summary>
    /// <param name="id">主键</param>
    Task Delete(Guid id);

    /// <summary>
    /// 根据主键更新指定本题纠错
    /// </summary>
    /// <param name="id">主键</param>
    /// <param name="model">新增模型</param>
    Task Update(Guid id, UpdateErrorCorrectingViewModel model);
}