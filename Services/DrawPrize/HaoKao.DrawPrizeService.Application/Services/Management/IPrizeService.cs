namespace HaoKao.DrawPrizeService.Application.Services.Management;

public interface IPrizeService : IAppWebApiService, IManager
{
    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    Task<BrowsePrizeViewModel> Get(Guid id);

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    Task<PrizeQueryViewModel> Get(PrizeQueryViewModel queryViewModel);

    /// <summary>
    /// 创建奖品
    /// </summary>
    /// <param name="model">新增模型</param>
    Task Create(CreatePrizeViewModel model);

    /// <summary>
    /// 根据主键删除指定奖品
    /// </summary>
    /// <param name="id">主键</param>
    Task Delete(Guid id);

    /// <summary>
    /// 根据主键更新指定奖品
    /// </summary>
    /// <param name="model">新增模型</param>
    Task Update(UpdatePrizeViewModel model);

    /// <summary>
    /// 根据主键更新指定奖品保底
    /// </summary>
    /// <param name="model">新增模型</param>
    Task SetPrizeGuaranteed(SetPrizeGuaranteedPrizeViewModel model);

    /// <summary>
    /// 根据主键取消指定奖品保底
    /// </summary>
    /// <param name="id"></param>
    Task CancelPrizeGuaranteed(Guid id);
}