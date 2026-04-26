namespace HaoKao.DataDictionaryService.Domain.Commands.Dictionaries;

public class DictionariesDataCommandModel
{
    /// <summary>
    /// 行号
    /// </summary>
    [Description("行号")]
    public int Index { get; set; }

    /// <summary>
    /// 编码
    /// </summary>
    [Required, Description("字典编码")]
    public string Code { get; set; }

    /// <summary>
    /// 值名称
    /// </summary>
    [Required, Description("字典名称")]
    public string Name { get; set; }

    /// <summary>
    /// 父节点编码
    /// </summary>
    [Description("父字典编码")]
    public string PCode { get; set; }

    private string pName;

    /// <summary>
    /// 父节点名称
    /// </summary>
    [Description("父字典名称")]
    public string PName
    {
        get => pName;
        set => pName = string.IsNullOrEmpty(value) ? "顶级节点" : value;
    }

    /// <summary>
    /// 状态:0为未启动 1为启用
    /// </summary>
    [Required, Description("状态")]
    public int State { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    [Required, Description("排序")]
    public int Sort { get; set; }
}