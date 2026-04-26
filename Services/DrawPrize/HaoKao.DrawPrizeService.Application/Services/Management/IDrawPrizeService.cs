namespace HaoKao.DrawPrizeService.Application.Services.Management;

public interface IDrawPrizeService : IAppWebApiService, IManager
{
    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    Task<BrowseDrawPrizeViewModel> Get(Guid id);

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    Task<DrawPrizeQueryViewModel> Get(DrawPrizeQueryViewModel queryViewModel);

    /// <summary>
    /// 创建抽奖
    /// </summary>
    /// <param name="model">新增模型</param>
    Task Create(CreateDrawPrizeViewModel model);

    /// <summary>
    /// 根据主键删除指定抽奖
    /// </summary>
    /// <param name="ids">主键</param>
    Task Delete(Guid[] ids);

    /// <summary>
    /// 根据主键更新指定抽奖
    /// </summary>
    /// <param name="model">新增模型</param>
    Task Update(UpdateDrawPrizeViewModel model);

    /// <summary>
    /// 设置抽奖规则
    /// </summary>
    /// <param name="model">新增模型</param>
    Task SetDrawPrizeRule(SetDrawPrizeRuleViewModel model);

    /// <summary>
    /// 启用
    /// </summary>
    /// <returns></returns>
    Task Enable(Guid[] modelIds);

    /// <summary>
    /// 禁用
    /// </summary>
    /// <param name="modelIds"></param>
    /// <returns></returns>
    Task Disable(Guid[] modelIds);
}