using Girvs.AuthorizePermission.Enumerations;
using HaoKao.Common;
using HaoKao.GroupBookingService.Domain.Entities;
using HaoKao.GroupBookingService.Domain.Repositories;
using HaoKao.GroupBookingService.Application.ViewModels.GroupSituation;
using System.Linq;
using HaoKao.GroupBookingService.Domain.Queries.EntityQuery;
using HaoKao.Common.Services;
using HaoKao.GroupBookingService.Domain.Extensions;

namespace HaoKao.GroupBookingService.Application.Services.Management;

/// <summary>
/// 接口服务
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.Management)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsJwt)]
[ServicePermissionDescriptor(
    "拼团情况",
    "9b93563a-ccc6-8fc4-92b8-73493297177c",
    "512",
    SystemModule.ExtendModule2,
    2
)]
public class GroupSituationService(
    IGroupSituationRepository repository,
    IStaticCacheManager cacheService) : IGroupSituationService
{
    #region 初始参数

    private readonly IGroupSituationRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly IStaticCacheManager _cacheManager = cacheService ?? throw new ArgumentNullException(nameof(cacheService));

    #endregion

    #region 查询 服务方法

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    [HttpGet]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View,
                                       UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<GroupSituationQueryViewModel> Get([FromQuery] GroupSituationQueryViewModel queryViewModel)
    {
        var query = queryViewModel.MapToQuery<GroupSituationQuery>();
        await _repository.GetSituationAndMemberAsync(query);

        return query.MapToQueryDto<GroupSituationQueryViewModel, GroupSituation>();
    }

    /// <summary>
    /// 查询拼团成功信息，分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    [HttpGet]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View,
                                       UserType.TenantAdminUser | UserType.GeneralUser)]
    [Obsolete]
    public async Task<SituationMemberQueryViewModel> GetGroupSituationAndMember([FromQuery] SituationMemberQueryViewModel queryViewModel)
    {
        var query = queryViewModel.MapToQuery<GroupSituationMemberQuery>();
        var tempQuery = await _cacheManager.GetAsync(GirvsEntityCacheDefaults<GroupSituation>.QueryCacheKey.Create($"GroupSituationById{query.GetCacheKey()}"), async () => 
        {
            await _repository.GetSuccessGroupSituationAsync(query);
            return query;
        });
      
        if (!query.Equals(tempQuery))
        {
            query.Result=tempQuery.Result;
            query.RecordCount = tempQuery.RecordCount;
        }

        return query.MapToQueryDto<SituationMemberQueryViewModel, GroupSituation>();
    }

    /// <summary>
    /// 通过用户Id获取已拼团成功数据
    /// </summary>
    /// <param name="UserId">用户Id</param>
    /// <returns></returns>
    [HttpGet]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View,
                                       UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<dynamic> GetUserGroupDataById(Guid UserId)
    {
       
        var memberGroups = await _cacheManager.GetAsync(GroupSituationCacheManager.MySuccessGroupSituationCacheKey.Create(UserId.ToString()), async () => 
        {
          return  await _repository.GetUserSuccessGroupSituationById(UserId);
        });
        return memberGroups.Select(t => new
        {
            t.DataName,
            t.SuitableSubjects,
            t.CreateTime,
            t.SuccessTime,
        }).OrderBy(t => t.CreateTime);
    }

    #endregion
}