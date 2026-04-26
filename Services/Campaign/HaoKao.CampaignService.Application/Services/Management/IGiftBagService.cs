using HaoKao.CampaignService.Application.ViewModels.GiftBag;
using HaoKao.CampaignService.Domain.ReceiveRules;

namespace HaoKao.CampaignService.Application.Services.Management;

public interface IGiftBagService : IAppWebApiService, IManager
{
    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    /// <returns></returns>
    Task<BrowseGiftBagViewModel> Get(Guid id);

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    /// <returns></returns>
    Task<QueryGiftBagViewModel> Get(QueryGiftBagViewModel queryViewModel);

    /// <summary>
    /// 获取礼包领取规则
    /// </summary>
    /// <param name="rules">规则</param>
    /// <returns></returns>
    IReadOnlyList<IReceiveRule> GetReceiveRules(IEnumerable<IReceiveRule> rules);

    /// <summary>
    /// 创建礼品包
    /// </summary>
    /// <param name="model">新增模型</param>
    /// <returns></returns>
    Task Create(CreateGiftBagViewModel model);

    /// <summary>
    /// 删除礼品包
    /// </summary>
    /// <param name="ids">主键</param>
    /// <returns></returns>
    Task Delete(IReadOnlyList<Guid> ids);

    /// <summary>
    /// 根据主键更新礼品包
    /// </summary>
    /// <param name="model">更新模型</param>
    /// <returns></returns>
    Task Update(UpdateGiftBagViewModel model);

    /// <summary>
    /// 修改发布状态
    /// </summary>
    /// <param name="model">更新模型</param>
    /// <returns></returns>
    Task Publish(UpdateGiftBagPublishedViewModel model);
}