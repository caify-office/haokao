namespace HaoKao.DataDictionaryService.Application.ViewModels.ClientModel;

public class UserInformationListViewModel : IDto
{
    /// <summary>
    /// 身份证号
    /// </summary>
    [JsonProperty(PropertyName = "cardId")]
    public string CardId { get; set; }

    /// <summary>
    /// 信息项id
    /// </summary>
    [JsonProperty(PropertyName = "informationItemId")]
    public Guid? InformationItemId { get; set; }

    /// <summary>
    /// 值(可能传的是 数据字典Id)
    /// </summary>
    [JsonProperty(PropertyName = "value")]
    public string Value { get; set; }

    /// <summary>
    /// 如果是Value是Guid，才需要数据字典值名称
    /// </summary>
    [JsonProperty(PropertyName = "display")]
    public string Display { get; set; }
}