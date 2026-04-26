namespace HaoKao.DataDictionaryService.Application.ViewModels.ClientModel;

public class DictionariesSupplyViewModel : IDto
{
    /// <summary>
    /// 文本
    /// </summary>
    [JsonProperty(PropertyName = "label")]
    public string Label { get; set; }

    /// <summary>
    /// 值
    /// </summary>
    [JsonProperty(PropertyName = "value")]
    public string Value { get; set; }

    /// <summary>
    /// 是否是子节点
    /// </summary>
    [JsonProperty(PropertyName = "isLeaf")]
    public bool? IsLeaf { get; set; }
}