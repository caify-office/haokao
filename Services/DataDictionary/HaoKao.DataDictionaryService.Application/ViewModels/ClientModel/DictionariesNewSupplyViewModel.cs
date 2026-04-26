namespace HaoKao.DataDictionaryService.Application.ViewModels.ClientModel;

[AutoMapFrom(typeof(Domain.Entities.Dictionaries))]
public class DictionariesNewSupplyViewModel : IDto
{
    /// <summary>
    /// 文本
    /// </summary>
    [JsonPropertyName(name: "label")]
    public string Name { get; set; }

    /// <summary>
    /// 值
    /// </summary>
    [JsonPropertyName(name: "value")]
    public Guid Id { get; set; }

    /// <summary>
    /// 是否是子节点
    /// </summary>
    public bool? IsLeaf { get; set; }
}