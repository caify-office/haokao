using HaoKao.ChapterNodeService.Domain.ChapterNodeModule;

namespace HaoKao.ChapterNodeService.Application.ChapterNodeModule.ViewModels;

[AutoMapTo(typeof(CreateChapterNodeCommand))]
public record CreateChapterNodeViewModel : IDto
{
    /// <summary>
    /// 科目Id
    /// </summary>
    [DisplayName("科目Id")]
    [Required(ErrorMessage = "{0}不能为空")]
    public Guid SubjectId { get; init; }

    /// <summary>
    /// 编码
    /// </summary>
    [DisplayName("编码")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MinLength(2, ErrorMessage = "{0}长度不能小于{1}")]
    [MaxLength(50, ErrorMessage = "{0}长度不能大于{1}")]
    public string Code { get; init; }

    /// <summary>
    /// 章节名称
    /// </summary>
    [DisplayName("章节名称")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MinLength(2, ErrorMessage = "{0}长度不能小于{1}")]
    [MaxLength(50, ErrorMessage = "{0}长度不能大于{1}")]
    public string Name { get; init; }

    /// <summary>
    /// 父级Id
    /// </summary>
    [DisplayName("父级Id")]
    public Guid? ParentId { get; init; }

    /// <summary>
    /// 父级名称
    /// </summary>
    [DisplayName("父级名称")]
    public string ParentName { get; init; }

    /// <summary>
    /// 排序号
    /// </summary>
    [DisplayName("排序号")]
    [Required(ErrorMessage = "{0}不能为空")]
    public int Sort { get; init; }
}