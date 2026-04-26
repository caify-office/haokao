namespace HaoKao.DataDictionaryService.Application.ViewModels.Dictionaries;

[AutoMapFrom(typeof(DictionariesQuery))]
[AutoMapTo(typeof(DictionariesQuery))]
public class DictionariesQueryViewModel : QueryDtoBase<DictionariesQueryListViewModel>
{
    /// <summary>
    /// 主键
    /// </summary>
    public Guid? Id { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 状态true/false
    /// </summary>
    public bool? State { get; set; }
}