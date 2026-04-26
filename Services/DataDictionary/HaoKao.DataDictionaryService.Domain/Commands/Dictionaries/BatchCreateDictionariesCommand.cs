namespace HaoKao.DataDictionaryService.Domain.Commands.Dictionaries;

/// <summary>
/// 批量创建数据字典值
/// </summary>
/// <param name="DictionariesDataCommandModels">数据字典值集合</param>
public record BatchCreateDictionariesCommand(IList<DictionariesDataCommandModel> DictionariesDataCommandModels) : Command(
    "批量创建数据字典值");

public class DetailItem
{
    [JsonProperty(PropertyName = "index")]
    public int Index { get; set; }

    [JsonProperty(PropertyName = "reason")]
    public List<string> Reason = [];
}

public class ImportResult
{
    [JsonProperty(PropertyName = "total")]
    public int Total { get; set; }

    [JsonProperty(PropertyName = "succNum")]
    public int SuccNum { get; set; }

    [JsonProperty(PropertyName = "failNum")]
    public int FailNum { get; set; }

    [JsonProperty(PropertyName = "detail")]
    public List<DetailItem> Detail = [];
}