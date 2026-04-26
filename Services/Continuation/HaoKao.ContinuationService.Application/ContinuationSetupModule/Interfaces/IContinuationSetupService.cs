using HaoKao.ContinuationService.Application.ContinuationSetupModule.ViewModels;

namespace HaoKao.ContinuationService.Application.ContinuationSetupModule.Interfaces;

public interface IContinuationSetupService : IAppWebApiService, IManager
{
    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    Task<BrowseContinuationSetupViewModel> Get(Guid id);

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="viewModel">查询对象</param>
    Task<QueryContinuationSetupViewModel> Get(QueryContinuationSetupViewModel viewModel);

    /// <summary>
    /// 检查同一个产品是否同时出现在有交集的时间段内
    /// </summary>
    /// <param name="viewModel"></param>
    /// <returns></returns>
    Task<IEnumerable<Guid>> CheckProducts(CheckProductsViewModel viewModel);

    /// <summary>
    /// 创建续读配置
    /// </summary>
    /// <param name="model">新增模型</param>
    Task Create(CreateContinuationSetupViewModel model);

    /// <summary>
    /// 根据主键删除指定续读配置
    /// </summary>
    /// <param name="id">主键</param>
    Task Delete(Guid id);

    /// <summary>
    /// 根据主键更新指定续读配置
    /// </summary>
    /// <param name="id">主键</param>
    /// <param name="model">新增模型</param>
    Task Update(Guid id, UpdateContinuationSetupViewModel model);

    /// <summary>
    /// 启用/禁用续读配置
    /// </summary>
    /// <param name="id">主键</param>
    /// <param name="enable">启用/禁用</param>
    Task Enable(Guid id, bool enable);

    /// <summary>
    /// 设置续读后会员到期时间
    /// </summary>
    /// <param name="id">主键</param>
    /// <param name="expiryTime">会员到期时间</param>
    Task UpdateExpiryTime(Guid id, DateTime expiryTime);
}