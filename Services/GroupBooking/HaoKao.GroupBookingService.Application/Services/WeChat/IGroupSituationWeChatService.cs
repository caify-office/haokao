using HaoKao.GroupBookingService.Application.ViewModels.GroupData;
using HaoKao.GroupBookingService.Application.ViewModels.GroupSituation;

namespace HaoKao.GroupBookingService.Application.Services.WeChat;

public interface IGroupSituationWeChatService : IAppWebApiService, IManager
{
    /// <summary>
    /// 创建
    /// </summary>
    /// <param name="model">新增模型</param>
    Task<string> Create(CreateGroupSituationViewModel model);

    Task JoinGroup([FromBody] GroupMemberJoinViewModel model);

    /// <summary>
    /// 获取数据-用户拼团资料情况列表
    /// </summary>
    /// <param name="subjectIds">科目Id</param>
    /// <param name="takeCount">获取数量</param>
    /// <returns></returns>
    Task<List<GroupDataListMobileViewModel>> GetGroupInformation([FromBody] Guid[] subjectIds, int? takeCount);

    Task<GroupSituationQueryListViewModel> GetIncludeAllByIdAsync(Guid groupSituationId);
}