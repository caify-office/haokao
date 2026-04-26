namespace HaoKao.DataDictionaryService.Application.ViewModels.Import;

[Serializable]
public class DataDictionaryItem
{
    [JsonProperty(PropertyName = "index")]
    public int Index { get; set; }

    /// <summary>
    /// 编码
    /// </summary>
    [JsonProperty(PropertyName = "code")]
    public string Code { get; set; }

    /// <summary>
    /// 值名称
    /// </summary>
    [JsonProperty(PropertyName = "name")]
    public string Name { get; set; }

    /// <summary>
    /// 父节点编码
    /// </summary>
    [JsonProperty(PropertyName = "pCode")]
    public string PCode { get; set; }

    /// <summary>
    /// 父节点名称
    /// </summary>
    [JsonProperty(PropertyName = "pName")]
    public string PName { get; set; }

    /// <summary>
    /// 状态:0为未启动 1为启用
    /// </summary>
    [JsonProperty(PropertyName = "state")]
    public int State { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    [JsonProperty(PropertyName = "sort")]
    public int Sort { get; set; }
}