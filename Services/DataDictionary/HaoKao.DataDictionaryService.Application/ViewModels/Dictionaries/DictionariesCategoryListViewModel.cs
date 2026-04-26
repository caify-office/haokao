namespace HaoKao.DataDictionaryService.Application.ViewModels.Dictionaries;

/// <summary>
/// 字典分类列表
/// </summary>
[AutoMapFrom(typeof(Domain.Entities.Dictionaries))]
public class DictionariesCategoryListViewModel : IDto
{
    /// <summary>
    /// 主键id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// 分组编码
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// 值名称
    /// </summary>
    public string Name { get; set; }
}