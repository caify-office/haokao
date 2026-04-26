using Girvs.Infrastructure;

namespace HaoKao.DataDictionaryService.Application.ViewModels.Dictionaries;

/// <summary>
/// 字典列表
/// </summary>
[AutoMapFrom(typeof(Domain.Entities.Dictionaries))]
public class DictionariesQueryListViewModel : IDto
{
    /// <summary>
    /// 主键id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; }

    /// <summary>
    /// 分组编码
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// 值名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 父节点id (吴勇飞修改20211129_1500：Pid做列表查询时不能忽略，因为前端界面修改字典时，需要拿去这个值再回传给后台)
    /// </summary>
    [JsonPropertyName(name: "pid")]
    public Guid? Pid { get; set; }

    /// <summary>
    /// 父节点名称
    /// </summary>
    public string PName { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    public bool? State { get; set; }

    /// <summary>
    /// 租户Id
    /// </summary>
    public Guid TenantId { get; set; }

    /// <summary>
    /// 是否共享
    /// </summary>
    public bool IsShare => TenantId.ToString() != EngineContext.Current.ClaimManager.IdentityClaim.TenantId;

    /// <summary>
    /// 是否系统管理员创建的公用信息
    /// </summary>
    public bool IsPublic => TenantId == Guid.Empty;
}