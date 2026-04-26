using HaoKao.GroupBookingService.Domain.Enumerations;
using HaoKao.GroupBookingService.Domain.Queries.EntityQuery;
using System.Linq;

namespace HaoKao.GroupBookingService.Application.ViewModels.GroupSituation;

[AutoMapFrom(typeof(GroupSituationQuery))]
[AutoMapTo(typeof(GroupSituationQuery))]
public class GroupSituationQueryViewModel : QueryDtoBase<GroupSituationQueryListViewModel>
{
    /// <summary>
    /// 资料名称
    /// </summary>
    public string DataName { get; set; }

    /// <summary>
    /// 拼团资料Id
    /// </summary>
    public Guid? GroupDataId { get; set; }

    /// <summary>
    /// 适用科目Id
    /// </summary>
    public Guid? SubjectId { get; set; }

    /// <summary>
    /// 团长/成员昵称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 拼团状态(1:成功  2:失败  3:拼团中)
    /// </summary>
    public GroupSituationStatus? Status { get; set; }

    /// <summary>
    /// 当前资料成功拼团人数总计(输入正确拼团资料Id和拼团状态为1，由后台统计返回)
    /// </summary>
    public int PeopleSuccessCount
    {
        get
        {
            if (Status is GroupSituationStatus.Success && GroupDataId.HasValue && Result != null && Result.Any())
            {
                var peopleNumber = Result.First().PeopleNumber;
                return peopleNumber * Result.Count;
            }
            return 0;
        }
    }
}

[AutoMapFrom(typeof(Domain.Entities.GroupSituation))]
[AutoMapTo(typeof(Domain.Entities.GroupSituation))]
public class GroupSituationQueryListViewModel : IDto
{
    /// <summary>
    /// 拼团情况Id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// 拼团资料Id
    /// </summary>
    public Guid GroupDataId { get; set; }

    /// <summary>
    /// 资料名
    /// </summary>
    public string DataName { get; set; }

    /// <summary>
    /// 适用科目
    /// </summary>
    public List<Guid> SuitableSubjects { get; set; }

    /// <summary>
    /// 成团人数
    /// </summary>
    public int PeopleNumber { get; set; }

    /// <summary>
    /// 限制时间
    /// </summary>
    public int LimitTime { get; set; }

    /// <summary>
    /// 拼团资料
    /// </summary>
    public string Document { get; set; }

    /// <summary>
    /// 拼团成功时间
    /// </summary>
    public DateTime? SuccessTime { get; set; }

    /// <summary>
    /// 开团时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 状态描述
    /// </summary>
    public string StatusStr
    {
        get
        {
            if (SuccessTime.HasValue)
            {
                return "已成功";
            }
            if (DateTime.Now > CreateTime.AddHours(LimitTime) && !SuccessTime.HasValue)
            {
                return "已失败";
            }
            if (DateTime.Now < CreateTime.AddHours(LimitTime) && !SuccessTime.HasValue)
            {
                return "拼团中";
            }
            return "未知";
        }
    }

    public int SecondsDown
    {
        get
        {
            var expired = CreateTime.AddHours(LimitTime);
            return Convert.ToInt32((expired - DateTime.Now).TotalSeconds);
        }
    }

    public List<QueryGroupMemberViewModel> GroupMembers { get; set; }
}

[AutoMapFrom(typeof(Domain.Entities.GroupMember))]
[AutoMapTo(typeof(Domain.Entities.GroupMember))]
public class QueryGroupMemberViewModel : IDto
{
    /// <summary>
    /// 用户Id
    /// </summary>
    public Guid CreatorId { get; set; }

    /// <summary>
    /// 昵称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 用户头像Url
    /// </summary>
    public string ImageUrl { get; set; }

    /// <summary>
    /// 是否团长
    /// </summary>
    public bool IsLeader { get; set; }
}