using HaoKao.CourseService.Domain.CourseModule;

namespace HaoKao.CourseService.Application.Modules.CourseModule.ViewModels;

[AutoMapTo(typeof(UpdateCourseCommand))]
public class UpdateCourseViewModel : IDto
{
    /// <summary>
    /// 主键
    /// </summary>
    [DisplayName("主键")]
    public Guid Id { get; init; }

    /// <summary>
    /// 课程名称
    /// </summary>
    [DisplayName("课程名称")]
    [Required(ErrorMessage = "{0}不能为空")]
    public string Name { get; init; }

    /// <summary>
    /// 主讲老师TeacherJson集合
    /// </summary>
    [DisplayName("主讲老师TeacherJson集合")]
    [Required(ErrorMessage = "{0}不能为空")]
    public string TeacherJson { get; init; }

    /// <summary>
    /// 年份
    /// </summary>
    [DisplayName("年份")]
    [Required(ErrorMessage = "{0}不能为空")]
    public string Year { get; init; }

    /// <summary>
    /// 科目id
    /// </summary>
    [DisplayName("科目id")]
    [Required(ErrorMessage = "{0}不能为空")]
    public Guid SubjectId { get; init; }

    /// <summary>
    /// 科目名称
    /// </summary>
    [DisplayName("科目名称")]
    [Required(ErrorMessage = "{0}不能为空")]
    public string SubjectName { get; init; }

    /// <summary>
    /// 上架状态 false-下架 true-上架
    /// </summary>
    [DisplayName("上架状态 false-下架 true-上架")]
    [Required(ErrorMessage = "{0}不能为空")]
    public bool State { get; init; }

    /// <summary>
    /// 预计更新时间
    /// </summary>
    [DisplayName("预计更新时间")]
    [Required(ErrorMessage = "{0}不能为空")]
    public string UpdateTimeDesc { get; init; }
}