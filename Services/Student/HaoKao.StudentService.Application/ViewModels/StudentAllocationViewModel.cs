using HaoKao.StudentService.Domain.Entities;
using HaoKao.StudentService.Domain.Queries;

namespace HaoKao.StudentService.Application.ViewModels;

[AutoMapTo(typeof(StudentAllocationQuery))]
[AutoMapFrom(typeof(StudentAllocationQuery))]
public class QueryStudentAllocationViewModel : QueryDtoBase<BrowseStudentAllocationViewModel>
{
    /// <summary>
    /// 手机号/昵称
    /// </summary>
    public string SearchKey { get; init; }

    /// <summary>
    /// 分配销售
    /// </summary>
    public string AllocateToSalesperson { get; init; }

    /// <summary>
    /// 已加销售
    /// </summary>
    public string FollowedSalesperson { get; init; }

    /// <summary>
    /// 分配时间(开始)
    /// </summary>
    public DateTime? StartAllocationTime { get; init; }

    /// <summary>
    /// 分配时间(结束)
    /// </summary>
    public DateTime? EndAllocationTime { get; init; }

    /// <summary>
    /// 添加状态
    /// </summary>
    public bool? IsFollowed { get; init; }
}

[AutoMapTo(typeof(StudentAllocation))]
[AutoMapFrom(typeof(StudentAllocation))]
public record BrowseStudentAllocationViewModel : IDto
{
    /// <summary>
    /// Id
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// 用户昵称
    /// </summary>
    public string NickName => Student.RegisterUser.NickName;

    /// <summary>
    /// 手机号码
    /// </summary>
    public string Phone => Student.RegisterUser.Phone;

    /// <summary>
    /// 注册时间
    /// </summary>
    public DateTime RegisterTime => Student.RegisterUser.CreateTime;

    /// <summary>
    /// 分配销售
    /// </summary>
    [JsonPropertyName("AllocateToSalesperson")]
    public string SalespersonName { get; init; }

    /// <summary>
    /// 已加销售
    /// </summary>
    public IReadOnlyList<string> FollowedSalesperson => Student.StudentFollows.Select(x => x.SalespersonName).ToList();

    /// <summary>
    /// 分配时间
    /// </summary>
    public DateTime AllocationTime { get; init; }

    /// <summary>
    /// 备注
    /// </summary>
    public string Remark { get; init; }

    /// <summary>
    /// 学员
    /// </summary>
    [JsonIgnore]
    public Student Student { get; init; }
}

/// <summary>
/// 更新学员分配给销售视图模型
/// </summary>
/// <param name="Ids"></param>
/// <param name="SalespersonId"></param>
/// <param name="SalespersonName"></param>
public record UpdateAllocateToViewModel(IReadOnlyList<Guid> Ids, Guid SalespersonId, string SalespersonName);

/// <summary>
/// 更新备注视图模型
/// </summary>
/// <param name="Id">Id</param>
/// <param name="Remark">备注</param>
public record UpdateRemarkViewModel(Guid Id, string Remark);