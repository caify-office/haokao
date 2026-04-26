using HaoKao.CourseService.Domain.CourseMaterialsModule;

namespace HaoKao.CourseService.Application.Modules.CourseMaterialsModule.ViewModels;

[AutoMapTo(typeof(CreateCourseMaterialsCommand))]
public record CreateCourseMaterialsViewModel : IDto
{
    /// <summary>
    /// 关联的课程章节id（阶段学习为课程章节id,智慧辅助学习为课程Id）
    /// </summary>
    [DisplayName("关联的课程章节id（阶段学习为课程章节id,智慧辅助学习为课程Id）")]
    [Required(ErrorMessage = "{0}不能为空")]
    public Guid CourseChapterId { get; init; }

    /// <summary>
    /// 关联的知识点id（智辅课程专用）
    /// </summary>
    [DisplayName("关联的知识点id（智辅课程专用）")]
    public Guid KnowledgePointId { get; set; }

    /// <summary>
    /// 讲义名称
    /// </summary>
    [DisplayName("讲义名称")]
    [Required(ErrorMessage = "{0}不能为空")]
    public string Name { get; init; }

    /// <summary>
    /// 讲义地址
    /// </summary>
    [DisplayName("讲义地址")]
    [Required(ErrorMessage = "{0}不能为空")]
    public string FileUrl { get; init; }
}

[AutoMapTo(typeof(SaveCourseMaterialsCommand))]
public record SaveCourseMaterialsViewModel : CreateCourseMaterialsViewModel;