namespace HaoKao.DataDictionaryService.Application.ViewModels.Import;

public class DataDictionaryFormatViewModel
{
    /// <summary>
    /// 报名序号
    /// </summary>
    [JsonProperty(PropertyName = "title")]
    public string Title { get; set; }

    [JsonProperty(PropertyName = "key")]
    public string Key { get; set; }

    [JsonProperty(PropertyName = "must")]
    public bool Must { get; set; }
}