using HaoKao.FeedBackService.Application.ViewModels.FeedBack;

namespace HaoKao.FeedBackService.Application.Services.Management;

public interface IFeedBackService : IAppWebApiService, IManager
{
    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    Task<BrowseFeedBackViewModel> Get(Guid id);

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    Task<FeedBackQueryViewModel> Get(FeedBackQueryViewModel queryViewModel);

    /// <summary>
    /// 读取用户提交次数
    /// </summary>
    /// <returns></returns>
    Task<int> GetUserCount();

    /// <summary>
    /// 根据主键删除指定
    /// </summary>
    /// <param name="id">主键</param>
    Task Delete(Guid id);

    /// <summary>
    /// 根据主键更新指定
    /// </summary>
    /// <param name="id">主键</param>
    /// <param name="model">新增模型</param>
    Task Update(Guid id, UpdateFeedBackViewModel model);

    /// <summary>
    /// 创建
    /// </summary>
    /// <param name="model">新增模型</param>
    Task Create([FromBody] CreateFeedBackViewModel model);
}