using HaoKao.LiveBroadcastService.Application.ViewModels.MutedUser;

namespace HaoKao.LiveBroadcastService.Application.Services.Management;

public interface IMutedUserService : IAppWebApiService, IManager
{
    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="viewModel">查询对象</param>
    Task<QueryMutedUserViewModel> Get(QueryMutedUserViewModel viewModel);

    /// <summary>
    /// 创建禁言用户
    /// </summary>
    /// <param name="model">新增模型</param>
    Task Create(CreateMutedUserViewModel model);

    /// <summary>
    /// 根据主键删除指定禁言用户
    /// </summary>
    /// <param name="userId">主键</param>
    Task Delete(Guid userId);

    /// <summary>
    /// 是否被禁言
    /// </summary>
    /// <returns></returns>
    Task<bool> IsMuted();
}