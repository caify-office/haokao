namespace HaoKao.CourseService.Application.Modules.CourseMaterialsModule.ViewModels;

public record SetCourseMaterialsSortViewModel : IDto
{
    /// <summary>
    /// Id
    /// </summary>
    [DisplayName("Id")]
    public Guid Id { get; init; }

    /// <summary>
    /// 排序
    /// </summary>
    [DisplayName("排序")]
    [Required(ErrorMessage = "{0}不能为空")]
    public int Sort { get; init; }
}