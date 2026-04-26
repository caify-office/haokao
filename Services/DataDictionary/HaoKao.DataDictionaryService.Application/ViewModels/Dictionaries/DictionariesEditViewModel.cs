namespace HaoKao.DataDictionaryService.Application.ViewModels.Dictionaries;

[AutoMapFrom(typeof(Domain.Entities.Dictionaries))]
public class DictionariesEditViewModel : IDto
{
    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; }

    /// <summary>
    /// 分组编码
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// 值名称
    /// </summary>
    [DisplayName("值名称")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MinLength(1, ErrorMessage = "{0}不能小于1")]
    [MaxLength(225, ErrorMessage = "{0}不能大于225")]
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