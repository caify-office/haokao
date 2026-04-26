using HaoKao.GroupBookingService.Domain.Queries.EntityQuery;
using Newtonsoft.Json;
using System.ComponentModel;

namespace HaoKao.GroupBookingService.Application.ViewModels.GroupData;

[AutoMapFrom(typeof(GroupDataQuery))]
[AutoMapTo(typeof(GroupDataQuery))]
public class GroupDataQueryViewModel : QueryDtoBase<GroupDataQueryListViewModel>
{
    /// <summary>
    /// 资料名
    /// </summary>
    public string DataName { get; set; }

    /// <summary>
    /// 适用科目Id
    /// </summary>
    public Guid? SubjectId { get; set; }
}

[AutoMapFrom(typeof(Domain.Entities.GroupData))]
[AutoMapTo(typeof(Domain.Entities.GroupData))]
public class GroupDataQueryListViewModel : IDto
{
    /// <summary>
    /// 拼团资料Id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// 团名
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
    /// 基础拼团成功人数
    /// </summary>
    public int BasePeopleNumber { get; set; }

    /// <summary>
    /// 限制时间
    /// </summary>
    public int LimitTime { get; set; }

    /// <summary>
    /// 拼团资料
    /// </summary>
    public string Document { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    public bool State { get; set; }
}

[AutoMapFrom(typeof(Domain.Entities.GroupData))]
[AutoMapTo(typeof(Domain.Entities.GroupData))]
public class GroupDataMobileQueryViewModel : IDto
{
    /// <summary>
    /// 拼团资料Id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// 资料名
    /// </summary>
    public string DataName { get; set; }

    /// <summary>
    /// 拼团总人数
    /// </summary>
    public int MemberCount { get; set; }

    /// <summary>
    /// 用户拼团状态
    /// </summary>
    public string Status { get; set; }

    /// <summary>
    /// 拼团情况Id
    /// </summary>
    public Guid? GroupSituationId { get; set; }
}

public class GroupDataListMobileViewModel : IDto
{
    /// <summary>
    /// 拼团资料Id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// 资料名
    /// </summary>
    public string DataName { get; set; }

    /// <summary>
    /// 拼团总人数
    /// </summary>
    public int MemberCount => SuccessCount * PeopleNumber + BasePeopleNumber;

    /// <summary>
    /// 用户拼团状态
    /// </summary>
    public GroupStatus Status
    {
        get
        {
            if (SuccessGroupSituationId == Guid.Empty && InGroupSituationId == Guid.Empty && FaildGroupSituationId == Guid.Empty)
            {
                return GroupStatus.WaitGroup;
            }
            if (SuccessGroupSituationId != Guid.Empty)
            {
                return GroupStatus.SuccessGroup;
            }
            if (SuccessGroupSituationId == Guid.Empty && InGroupSituationId != Guid.Empty)
            {
                return GroupStatus.InGroup;
            }
            return GroupStatus.FaildGroup;
        }
    }

    /// <summary>
    /// 拼团情况Id
    /// </summary>
    public Guid? GroupSituationId
    {
        get
        {
            if (SuccessGroupSituationId == Guid.Empty && InGroupSituationId == Guid.Empty && FaildGroupSituationId == Guid.Empty)
            {
                return null;
            }
            if (SuccessGroupSituationId != Guid.Empty)
            {
                return SuccessGroupSituationId.Value;
            }
            if (SuccessGroupSituationId == Guid.Empty && InGroupSituationId != Guid.Empty)
            {
                return InGroupSituationId.Value;
            }
            return FaildGroupSituationId.Value;
        }
    }

    /// <summary>
    /// 拼团成功 团数量
    /// </summary>
    [JsonIgnore]
    public int SuccessCount { get; set; }

    /// <summary>
    /// 成团人数
    /// </summary>
    [JsonIgnore]
    public int PeopleNumber { get; set; }

    [JsonIgnore]
    public int BasePeopleNumber { get; set; }

    /// <summary>
    /// 拼团成功 团Id
    /// </summary>
    [JsonIgnore]
    public Guid? SuccessGroupSituationId { get; set; }

    /// <summary>
    /// 拼团中 团Id
    /// </summary>
    [JsonIgnore]
    public Guid? InGroupSituationId { get; set; }

    /// <summary>
    /// 拼团失败 团Id
    /// </summary>
    [JsonIgnore]
    public Guid? FaildGroupSituationId { get; set; }
}

/// <summary>
/// 拼团状态枚举
/// </summary>
[Description("拼团状态")]
public enum GroupStatus
{
    /// <summary>
    /// 待拼团
    /// </summary>
    [Description("待拼团")]
    WaitGroup,

    /// <summary>
    /// 拼团中
    /// </summary>
    [Description("拼团中")]
    InGroup,

    /// <summary>
    /// 拼团成功
    /// </summary>
    [Description("拼团成功")]
    SuccessGroup,

    /// <summary>
    /// 拼团失败
    /// </summary>
    [Description("拼团失败")]
    FaildGroup,
}