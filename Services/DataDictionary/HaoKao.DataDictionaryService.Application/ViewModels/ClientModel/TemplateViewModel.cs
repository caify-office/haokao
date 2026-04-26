namespace HaoKao.DataDictionaryService.Application.ViewModels.ClientModel;

public class TemplateViewModel : IDto
{
    /// <summary>
    /// 类别
    /// </summary>
    [JsonProperty(PropertyName = "category")]
    public string Category { get; set; }

    /// <summary>
    /// 类别Id
    /// </summary>
    [JsonProperty(PropertyName = "categoryId")]
    public dynamic CategoryId { get; set; }

    /// <summary>
    /// 类别描述
    /// </summary>
    [JsonProperty(PropertyName = "description")]
    public string Description { get; set; }

    [JsonProperty(PropertyName = "list")]
    public List<ListItem> List { get; set; }
}

public class ListItem : IDto
{
    /// <summary>
    /// Id
    /// </summary>
    [JsonProperty(PropertyName = "id")]
    public string Id { get; set; }

    /// <summary>
    /// 代码
    /// </summary>
    [JsonProperty(PropertyName = "code")]
    public string Code { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    [JsonProperty(PropertyName = "name")]
    public string Name { get; set; }

    /// <summary>
    /// 字典Id
    /// </summary>
    [JsonProperty(PropertyName = "dictionaryId")]
    public string DictionaryId { get; set; }

    /// <summary>
    /// 字典名称
    /// </summary>
    [JsonProperty(PropertyName = "dictionaryName")]
    public string DictionaryName { get; set; }

    /// <summary>
    /// 补充字典数据
    /// </summary>
    [JsonProperty(PropertyName = "dataSource")]
    public List<DictionariesSupplyViewModel> DataSource { get; set; } = [];

    public List<DictionariesNewSupplyViewModel> NewDataSource { get; set; } = [];

    /// <summary>
    /// 类型
    /// </summary>
    [JsonProperty(PropertyName = "type")]
    public int Type { get; set; }

    /// <summary>
    /// 类型代码
    /// </summary>
    [JsonProperty(PropertyName = "typeCode")]
    public string TypeCode { get; set; }

    [JsonProperty(PropertyName = "typeName")]
    public string TypeName { get; set; }

    [JsonProperty(PropertyName = "validates")]
    public List<dynamic> Validates { get; set; }

    [JsonProperty(PropertyName = "infoTypeId")]
    public string InfoTypeId { get; set; }

    /// <summary>
    /// 信息类型
    /// </summary>
    [JsonProperty(PropertyName = "infoType")]
    public int InfoType { get; set; }

    [JsonProperty(PropertyName = "isPublic")]
    public string IsPublic { get; set; }

    [JsonProperty(PropertyName = "tenantId")]
    public string TenantId { get; set; }

    /// <summary>
    /// 信息类型名称
    /// </summary>
    [JsonProperty(PropertyName = "infoTypeName")]
    public string InfoTypeName { get; set; }

    /// <summary>
    /// 角色
    /// </summary>
    [JsonProperty(PropertyName = "rules")]
    public List<dynamic> Rules { get; set; }

    [JsonProperty(PropertyName = "remark")]
    public string Remark { get; set; }

    [JsonProperty(PropertyName = "isShare")]
    public string IsShare { get; set; }

    /// <summary>
    /// 描述
    /// </summary>
    [JsonProperty(PropertyName = "description")]
    public string Description { get; set; }

    /// <summary>
    /// 默认数据
    /// </summary>
    [JsonProperty(PropertyName = "defaultValue")]
    public string DefaultValue { get; set; }

    [JsonProperty(PropertyName = "placeholder")]
    public string Placeholder { get; set; }

    [JsonProperty(PropertyName = "class")]
    public List<dynamic> Class { get; set; }

    [JsonProperty(PropertyName = "attachmentConfig")]
    public dynamic AttachmentConfig { get; set; }
}