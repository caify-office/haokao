using HaoKao.CourseService.Domain.CourseChapterModule;

namespace HaoKao.CourseService.Application.Modules.CourseChapterModule.ViewModels;

[AutoMapTo(typeof(CreateCourseChapterCommand))]
public record CreateCourseChapterViewModel : IDto
{
    /// <summary>
    /// 名称
    /// </summary>
    [DisplayName("名称")]
    [Required(ErrorMessage = "{0}不能为空")]
    public string Name { get; init; }

    /// <summary>
    /// 父id
    /// </summary>
    [DisplayName("父id")]
    [Required(ErrorMessage = "{0}不能为空")]
    public Guid ParentId { get; init; }

    /// <summary>
    /// 关联的课程id
    /// </summary>
    [DisplayName("关联的课程id")]
    [Required(ErrorMessage = "{0}不能为空")]
    public Guid CourseId { get; init; }

    /// <summary>
    /// 是否叶子节点
    /// </summary>
    [DisplayName("是否叶子节点")]
    [Required(ErrorMessage = "{0}不能为空")]
    public bool IsLeaf { get; init; }

    /// <summary>
    /// 排序
    /// </summary>
    [DisplayName("排序")]
    public int Sort { get; init; }
}