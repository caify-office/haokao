namespace HaoKao.DataDictionaryService.Application.ViewModels.ClientModel;

public class NewTemplateViewModel : IDto
{
    /// <summary>
    /// 类别
    /// </summary>
    public string Category { get; set; }

    /// <summary>
    /// 类别Id
    /// </summary>

    public dynamic CategoryId { get; set; }

    /// <summary>
    /// 类别描述
    /// </summary>
    public string Description { get; set; }

    public List<NewListItem> List { get; set; }
}

public class NewListItem : IDto
{
    /// <summary>
    /// Id
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// 代码
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 字典Id
    /// </summary>
    public string DictionaryId { get; set; }

    /// <summary>
    /// 字典名称
    /// </summary>
    public string DictionaryName { get; set; }

    /// <summary>
    /// 补充字典数据
    /// </summary>
    public List<DictionariesNewSupplyViewModel> DataSource { get; set; } = [];

    /// <summary>
    /// 类型
    /// </summary>
    public int Type { get; set; }

    /// <summary>
    /// 类型代码
    /// </summary>
    public string TypeCode { get; set; }

    public string TypeName { get; set; }

    public List<dynamic> Validates { get; set; }

    public string InfoTypeId { get; set; }

    /// <summary>
    /// 信息类型
    /// </summary>
    public int InfoType { get; set; }

    public bool? IsPublic { get; set; }

    public string TenantId { get; set; }

    /// <summary>
    /// 信息类型名称
    /// </summary>
    public string InfoTypeName { get; set; }

    public List<dynamic> Rules { get; set; }

    public string Remark { get; set; }

    public bool? IsShare { get; set; }

    /// <summary>
    /// 描述
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// 默认数据
    /// </summary>
    public string DefaultValue { get; set; }

    public string Placeholder { get; set; }

    public List<dynamic> Class { get; set; }

    public dynamic AttachmentConfig { get; set; }
}