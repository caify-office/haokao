using HaoKao.StudentService.Domain.Entities;
using HaoKao.StudentService.Domain.Models;
using HaoKao.StudentService.Domain.Queries;

namespace HaoKao.StudentService.Application.ViewModels;

[AutoMapFrom(typeof(StudentQuery))]
[AutoMapTo(typeof(StudentQuery))]
public class QueryStudentViewModel : QueryDtoBase<BrowseStudentViewModel>
{
    /// <summary>
    /// 昵称/手机号码
    /// </summary>
    public string SearchKey { get; init; }

    /// <summary>
    /// 是否付费学员
    /// </summary>
    public bool? IsPaidStudent { get; init; }

    /// <summary>
    /// 是否加销售
    /// </summary>
    public bool? IsFollowedSalesperson { get; init; }

    /// <summary>
    /// 销售名称
    /// </summary>
    public string SalespersonName { get; init; }

    /// <summary>
    /// 注册开始时间
    /// </summary>
    public DateTime? StartTime { get; init; }

    /// <summary>
    /// 注册结束时间
    /// </summary>
    public DateTime? EndTime { get; init; }
}

[AutoMapTo(typeof(Student))]
[AutoMapFrom(typeof(Student))]
public record BrowseStudentViewModel : IDto
{
    /// <summary>
    /// Id
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// 用户Id
    /// </summary>
    public Guid RegisterUserId { get; init; }

    /// <summary>
    /// 昵称
    /// </summary>
    public string NickName => RegisterUser.NickName;

    /// <summary>
    /// 手机号码
    /// </summary>
    public string Phone => RegisterUser.Phone;

    /// <summary>
    /// 邮箱
    /// </summary>
    public string EmailAddress => RegisterUser.EmailAddress;

    /// <summary>
    /// 是否付费学员
    /// </summary>
    public bool IsPaidStudent { get; set; }

    /// <summary>
    /// 注册时间
    /// </summary>
    public DateTime RegisterTime => RegisterUser.CreateTime;

    /// <summary>
    /// 已加销售
    /// </summary>
    public IReadOnlyList<string> FollowedSalesperson => StudentFollows.Select(x => x.SalespersonName).ToList();

    /// <summary>
    /// 当前租户Id
    /// </summary>
    public Guid TenantId { get; init; }

    /// <summary>
    /// 注册用户
    /// </summary>
    public RegisterUser RegisterUser { get; init; }

    /// <summary>
    /// 已加销售
    /// </summary>
    [JsonIgnore]
    public IReadOnlyList<StudentFollow> StudentFollows { get; init; }
}