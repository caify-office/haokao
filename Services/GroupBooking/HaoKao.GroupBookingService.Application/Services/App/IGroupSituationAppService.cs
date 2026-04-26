using HaoKao.GroupBookingService.Application.ViewModels.GroupData;
using HaoKao.GroupBookingService.Application.ViewModels.GroupSituation;

namespace HaoKao.GroupBookingService.Application.Services.App;

public interface IGroupSituationAppService : IAppWebApiService, IManager
{
    /// <summary>
    /// 创建
    /// </summary>
    /// <param name="model">新增模型</param>
    Task<string> Create(CreateGroupSituationViewModel model);

    /// <summary>
    /// 加入拼团
    /// </summary>
    /// <param name="model">新增模型</param>
    Task JoinGroup(GroupMemberJoinViewModel model);

    /// <summary>
    /// 获取数据-拼团详细情况
    /// </summary>
    /// <param name="groupSituationId">拼团Id</param>
    /// <returns></returns>
    Task<GroupSituationQueryListViewModel> GetIncludeAllByIdAsync(Guid groupSituationId);

    /// <summary>
    /// 获取数据-用户拼团资料情况列表
    /// </summary>
    /// <param name="subjectIds">科目Id</param>
    /// <param name="takeCount">获取数量</param>
    /// <returns></returns>
    Task<List<GroupDataListMobileViewModel>> GetGroupInformation(Guid[] subjectIds, int? takeCount);

    /// <summary>
    /// 通过资料id获取资料信息
    /// </summary>
    /// <param name="groupDataId">资料Id</param>
    Task<BrowseGroupDataViewModel> GetGroupDataById(Guid groupDataId);
}