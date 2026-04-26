using HaoKao.OrderService.Application.ViewModels.PlatformPayer;

namespace HaoKao.OrderService.Application.Services.Management;

public interface IPlatformPayerService : IAppWebApiService, IManager
{
    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    Task<BrowsePlatformPayerViewModel> Get(Guid id);

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    Task<PlatformPayerQueryViewModel> Get(PlatformPayerQueryViewModel queryViewModel);

    /// <summary>
    /// 创建平台配置的支付列表
    /// </summary>
    /// <param name="model">新增模型</param>
    Task Create(CreatePlatformPayerViewModel model);

    /// <summary>
    /// 根据主键删除指定平台配置的支付列表
    /// </summary>
    /// <param name="id">主键</param>
    Task Delete(Guid id);

    /// <summary>
    /// 根据主键更新指定平台配置的支付列表
    /// </summary>
    /// <param name="id">主键</param>
    /// <param name="model">新增模型</param>
    Task Update(Guid id, UpdatePlatformPayerViewModel model);

    /// <summary>
    /// 启用
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task Enable(Guid id);

    /// <summary>
    /// 禁用
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task Disable(Guid id);
}