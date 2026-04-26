namespace HaoKao.DataDictionaryService.Application.ViewModels.Dictionaries;

[AutoMapFrom(typeof(Domain.Entities.Dictionaries))]
public class DictionariesTreeViewModel : IDto
{
    /// <summary>
    /// 主键Id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; }

    /// <summary>
    /// 编码分组
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// 值名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 父节点id
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
    public bool State { get; set; }

    /// <summary>
    /// 子类
    /// </summary>
    [System.Text.Json.Serialization.JsonIgnore]
    public List<DictionariesTreeViewModel> Children { get; set; }

    /// <summary>
    /// 是否叶子节点
    /// </summary>
    public bool? IsLeaf => !Children.Any();

    /// <summary>
    /// 租户Id
    /// </summary>
    public Guid TenantId { get; set; }

    /// <summary>
    /// 是否共享
    /// </summary>
    public bool IsShare => TenantId.ToString() != (Girvs.Infrastructure.EngineContext.Current.ClaimManager.IdentityClaim?.TenantId ?? Guid.Empty.ToString());

    /// <summary>
    /// 是否系统管理员创建的公用信息
    /// </summary>
    public bool IsPublic => TenantId == Guid.Empty;
}