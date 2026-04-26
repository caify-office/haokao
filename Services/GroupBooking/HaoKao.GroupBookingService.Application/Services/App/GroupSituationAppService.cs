using AutoMapper;
using Girvs.Driven.Extensions;
using HaoKao.Common;
using HaoKao.GroupBookingService.Application.Services.Management;
using HaoKao.GroupBookingService.Application.Services.WeChat;
using HaoKao.GroupBookingService.Application.ViewModels.GroupData;
using HaoKao.GroupBookingService.Application.ViewModels.GroupSituation;
using HaoKao.GroupBookingService.Domain.Commands.GroupSituation;
using HaoKao.GroupBookingService.Domain.Entities;
using HaoKao.GroupBookingService.Domain.Extensions;
using HaoKao.GroupBookingService.Domain.Repositories;

namespace HaoKao.GroupBookingService.Application.Services.App;

/// <summary>
/// 拼团接口服务-App
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.App)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
// [Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsJwt)]
public class GroupSituationAppService(
    IStaticCacheManager cacheManager,
    IMediatorHandler bus,
    INotificationHandler<DomainNotification> notifications,
    IGroupSituationRepository repository,
    IGroupDataRepository repositoryGroupData,
    IGroupDataService groupDataService,
    IGroupSituationWeChatService groupSituationWeChatService ) : IGroupSituationAppService
{
    #region 初始参数

    private readonly IStaticCacheManager _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly DomainNotificationHandler _notifications = (DomainNotificationHandler)notifications;
    private readonly IGroupSituationRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly IGroupDataRepository _groupDataRepository = repositoryGroupData ?? throw new ArgumentNullException(nameof(repositoryGroupData));
    private readonly IGroupDataService _groupDataService = groupDataService ?? throw new ArgumentNullException(nameof(groupDataService));
    private readonly IGroupSituationWeChatService _groupSituationWeChatService = groupSituationWeChatService ?? throw new ArgumentNullException(nameof(groupSituationWeChatService));

    #endregion

    #region 服务方法

    /// <summary>
    /// 创建
    /// </summary>
    /// <param name="model">新增模型</param>
    [HttpPost]
    public  Task<string> Create([FromBody] CreateGroupSituationViewModel model)
    {
        return _groupSituationWeChatService.Create( model );
    }

    /// <summary>
    /// 加入拼团
    /// </summary>
    /// <param name="model">新增模型</param>
    [HttpPost]
    public  Task JoinGroup([FromBody] GroupMemberJoinViewModel model)
    {
        return _groupSituationWeChatService.JoinGroup(model);
    }

    /// <summary>
    /// 获取数据-拼团详细情况
    /// </summary>
    /// <param name="groupSituationId">拼团Id</param>
    /// <returns></returns>
    [HttpGet]
    public  Task<GroupSituationQueryListViewModel> GetIncludeAllByIdAsync(Guid groupSituationId)
    {
        return _groupSituationWeChatService.GetIncludeAllByIdAsync(groupSituationId);
    }

    /// <summary>
    /// 获取数据-用户拼团资料情况列表
    /// </summary>
    /// <param name="subjectIds">科目Id</param>
    /// <param name="takeCount">获取数量</param>
    /// <returns></returns>
    [HttpPost]
    public  Task<List<GroupDataListMobileViewModel>> GetGroupInformation([FromBody] Guid[] subjectIds, int? takeCount)
    {
       return  _groupSituationWeChatService.GetGroupInformation(subjectIds, takeCount);
    }

   

    /// <summary>
    /// 获取数据-个人中心-我的资料
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<List<GroupDataListMobileViewModel>> GetMyGroupInformation()
    {
        var userId = EngineContext.Current.ClaimManager.GetUserId();
        var tenantId = EngineContext.Current.ClaimManager.GetTenantId();
        var cachekey = GroupDataCacheManager.MyGroupDataCacheKey.Create(userId);
        var result = await _cacheManager.GetAsync(cachekey, async () => await GetMyGroupInfo());
        return result;
    }

    private async Task<List<GroupDataListMobileViewModel>> GetMyGroupInfo()
    {
        var dataTable = await _groupDataRepository.GetMyGroupDataList();
        var resultList = new List<GroupDataListMobileViewModel>();
        foreach (DataRow dr in dataTable.Rows)
        {
            resultList.Add(new GroupDataListMobileViewModel
            {
                Id = Guid.Parse(dr["Id"].ToString()),
                DataName = dr["DataName"].ToString(),
                PeopleNumber = int.Parse(dr["PeopleNumber"].ToString()),
                BasePeopleNumber = int.Parse(dr["BasePeopleNumber"].ToString()),
                SuccessCount = int.Parse(dr["SuccessCount"].ToString()),
                SuccessGroupSituationId = dr["SuccessGroupSituationId"].ToString() == "" ? Guid.Empty : Guid.Parse(dr["SuccessGroupSituationId"].ToString()),
            });
        }

        return resultList;
    }

    /// <summary>
    /// 通过资料id获取资料信息
    /// </summary>
    /// <param name="groupDataId">资料Id</param>
    public  Task<BrowseGroupDataViewModel> GetGroupDataById(Guid groupDataId)
    {
      return  _groupDataService.Get(groupDataId);
    }

    #endregion
}