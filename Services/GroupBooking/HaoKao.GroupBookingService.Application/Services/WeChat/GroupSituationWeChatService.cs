using AutoMapper;
using Girvs.Driven.Extensions;
using HaoKao.Common;
using HaoKao.GroupBookingService.Application.Services.App;
using HaoKao.GroupBookingService.Application.ViewModels.GroupData;
using HaoKao.GroupBookingService.Application.ViewModels.GroupSituation;
using HaoKao.GroupBookingService.Domain.Commands.GroupSituation;
using HaoKao.GroupBookingService.Domain.Entities;
using HaoKao.GroupBookingService.Domain.Repositories;

namespace HaoKao.GroupBookingService.Application.Services.WeChat;

/// <summary>
/// 拼团接口服务-WeChat
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.WeChat)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
// [Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsJwt)]
public class GroupSituationWeChatService(
    IStaticCacheManager cacheManager,
    IMediatorHandler bus,
    INotificationHandler<DomainNotification> notifications,
    IGroupSituationRepository repository,
    IGroupDataRepository dataRepository,
    IMapper mapper
) : IGroupSituationWeChatService
{
    #region 初始参数

    private readonly IStaticCacheManager _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly DomainNotificationHandler _notifications = (DomainNotificationHandler)notifications;
    private readonly IGroupSituationRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly IGroupDataRepository _groupDataRepository = dataRepository ?? throw new ArgumentNullException(nameof(dataRepository));

    #endregion

    #region 服务方法

    /// <summary>
    /// 创建
    /// </summary>
    /// <param name="model">新增模型</param>
    [HttpPost]
    public async  Task<string> Create([FromBody] CreateGroupSituationViewModel model)
    {
        var command = mapper.Map<CreateGroupSituationCommand>(model);
        var contentId = await _bus.SendCommand<CreateGroupSituationCommand, Guid>(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
        return contentId.ToString();
    }

    /// <summary>
    /// 加入拼团
    /// </summary>
    /// <param name="model">新增模型</param>
    [HttpPost]
    public async  Task JoinGroup([FromBody] GroupMemberJoinViewModel model)
    {
        var command = model.MapToCommand<JoinGroupSituationCommand>();

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 获取数据-拼团详细情况
    /// </summary>
    /// <param name="groupSituationId">拼团Id</param>
    /// <returns></returns>
    [HttpGet]
    public async Task<GroupSituationQueryListViewModel> GetIncludeAllByIdAsync(Guid groupSituationId)
    {
        var key = GirvsEntityCacheDefaults<GroupSituation>.ByIdCacheKey.Create(groupSituationId.ToString());
        var result = await _cacheManager.GetAsync(key, () => _repository.GetByIdIncludeAllAsync(groupSituationId));
        return result.MapToDto<GroupSituationQueryListViewModel>();
    }

    /// <summary>
    /// 获取数据-用户拼团资料情况列表
    /// </summary>
    /// <param name="subjectIds">科目Id</param>
    /// <param name="takeCount">获取数量</param>
    /// <returns></returns>
    [HttpPost]
    [AllowAnonymous]
    public async Task<List<GroupDataListMobileViewModel>> GetGroupInformation([FromBody] Guid[] subjectIds, int? takeCount)
    {
        var dataTable = await _groupDataRepository.GetGroupDataListBySubjectId(subjectIds, takeCount);
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
                InGroupSituationId = dr["InGroupSituationId"].ToString() == "" ? Guid.Empty : Guid.Parse(dr["InGroupSituationId"].ToString()),
                FaildGroupSituationId = dr["FailedGroupSituationId"].ToString() == "" ? Guid.Empty : Guid.Parse(dr["FailedGroupSituationId"].ToString()),
            });
        }

        return resultList;
    }

    /// <summary>
    /// 通过资料id获取资料信息
    /// </summary>
    /// <param name="contentId">资料Id</param>
    public async Task<BrowseGroupDataViewModel> GetContentById(Guid contentId)
    {
        var groupData = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<GroupData>.ByIdCacheKey.Create(contentId.ToString()),
            async () => await _groupDataRepository.GetByIdAsync(contentId)
        );

        if (groupData == null)
        {
            throw new GirvsException("对应的不存在", StatusCodes.Status404NotFound);
        }

        return groupData.MapToDto<BrowseGroupDataViewModel>();
    }

    #endregion
}