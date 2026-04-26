namespace HaoKao.DataDictionaryService.Application.ViewModels.Dictionaries;

[AutoMapFrom(typeof(Domain.Entities.Dictionaries))]
public class DictionariesDetailViewModel : IDto
{
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
    public Guid? Pid { get; set; }

    /// <summary>
    /// 父节点名称
    /// </summary>
    public string PName { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    public bool State { get; set; }
}