using HaoKao.GroupBookingService.Domain.Queries.EntityQuery;

namespace HaoKao.GroupBookingService.Application.ViewModels.GroupSituation;

[AutoMapFrom(typeof(GroupSituationMemberQuery))]
[AutoMapTo(typeof(GroupSituationMemberQuery))]
public class SituationMemberQueryViewModel : QueryDtoBase<SituationMemberQueryListViewModel>
{
    /// <summary>
    /// 拼团资料Id
    /// </summary>
    public Guid GroupDataId { get; set; }
}

[AutoMapFrom(typeof(Domain.Entities.GroupSituation))]
[AutoMapTo(typeof(Domain.Entities.GroupSituation))]
public class SituationMemberQueryListViewModel : IDto
{
    /// <summary>
    /// 拼团成功时间
    /// </summary>
    public DateTime? SuccessTime { get; set; }

    /// <summary>
    /// 开团时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 成团人数
    /// </summary>
    public int PeopleNumber { get; set; }

    public List<QuerySituationMemberViewModel> GroupMembers { get; set; }
}

[AutoMapFrom(typeof(Domain.Entities.GroupMember))]
[AutoMapTo(typeof(Domain.Entities.GroupMember))]
public class QuerySituationMemberViewModel : IDto
{
    /// <summary>
    /// 用户Id
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// 昵称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 是否团长
    /// </summary>
    public bool IsLeader { get; set; }
}