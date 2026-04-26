using HaoKao.SubjectService.Domain.SubjectModule;

namespace HaoKao.SubjectService.Application.ViewModels;

[AutoMapFrom(typeof(SubjectQuery))]
[AutoMapTo(typeof(SubjectQuery))]
public class SubjectQueryViewModel : QueryDtoBase<BrowseSubjectViewModel>
{
    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    ///是否专业课
    /// </summary>
    public SubjectTypeEnum? IsCommon { get; set; }
}

[AutoMapFrom(typeof(Subject))]
[AutoMapTo(typeof(Subject))]
public record BrowseSubjectViewModel : IDto
{
    /// <summary>
    /// 科目Id
    /// </summary>
    public Guid? Id { get; init; }

    /// <summary>
    /// 科目名称
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// 普通科目/专业科目
    /// </summary>
    public SubjectTypeEnum IsCommon { get; init; }

    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; init; }

    /// <summary>
    /// 是否显示
    /// </summary>
    public bool IsShow { get; init; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; init; }
}

public record CreateSubjectViewModel : IDto
{
    /// <summary>
    /// 科目名称
    /// </summary>
    [DisplayName("科目名称")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MinLength(2, ErrorMessage = "{0}长度不能小于{1}")]
    [MaxLength(50, ErrorMessage = "{0}长度不能大于{1}")]
    public string Name { get; init; }

    /// <summary>
    /// 普通科目/专业科目
    /// </summary>
    [DisplayName("普通科目/专业科目")]
    [Required(ErrorMessage = "{0}不能为空")]
    public SubjectTypeEnum IsCommon { get; init; }

    /// <summary>
    /// 排序
    /// </summary>
    [DisplayName("排序")]
    [Required(ErrorMessage = "{0}不能为空")]
    public int Sort { get; init; }

    /// <summary>
    /// 是否显示
    /// </summary>
    [DisplayName("是否显示")]
    public bool IsShow { get; init; }
}

public record UpdateSubjectViewModel : CreateSubjectViewModel;