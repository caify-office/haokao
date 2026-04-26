using HaoKao.CourseService.Domain.CoursePracticeModule;

namespace HaoKao.CourseService.Application.Modules.CoursePracticeModule.ViewModels;

[AutoMapTo(typeof(CreateCoursePracticeCommand))]
public record CreateCoursePracticeViewModel : IDto
{
    /// <summary>
    /// 科目id
    /// </summary>
    public Guid SubjectId { get; set; }

    /// <summary>
    /// 关联的课程章节id（阶段学习为课程章节id,智慧辅助学习为课程Id）
    /// </summary>
    [DisplayName("关联的课程章节id（阶段学习为课程章节id,智慧辅助学习为课程Id）")]
    [Required(ErrorMessage = "{0}不能为空")]
    public Guid CourseChapterId { get; init; }

    /// <summary>
    /// 关联的课程章节名称（阶段学习为课程章节名称,智慧辅助学习为课程名称）
    /// </summary>
    [DisplayName("关联的课程章节名称（阶段学习为课程章节名称,智慧辅助学习为课程名称）")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MinLength(2, ErrorMessage = "{0}长度不能小于{1}")]
    [MaxLength(50, ErrorMessage = "{0}长度不能大于{1}")]
    public string CourseChapterName { get; init; }

    /// <summary>
    /// 关联的试题章节id（阶段课程专用）
    /// </summary>
    [DisplayName("关联的试题章节id")]
    public Guid? ChapterNodeId { get; init; }

    /// <summary>
    /// 关联的试题章节名称（阶段课程专用）
    /// </summary>
    [DisplayName("关联的试题章节名称")]
    [MinLength(2, ErrorMessage = "{0}长度不能小于{1}")]
    [MaxLength(50, ErrorMessage = "{0}长度不能大于{1}")]
    public string ChapterNodeName { get; init; }

    /// <summary>
    /// 关联的试题分类Id（阶段课程专用）
    /// </summary>
    [DisplayName("关联的试题分类Id")]
    public Guid? QuestionCategoryId { get; init; }

    /// <summary>
    /// 关联的试题分类名称（阶段课程专用）
    /// </summary>
    [DisplayName("关联的试题分类名称")]
    [MinLength(2, ErrorMessage = "{0}长度不能小于{1}")]
    [MaxLength(50, ErrorMessage = "{0}长度不能大于{1}")]
    public string QuestionCategoryName { get; init; }
}

[AutoMapTo(typeof(UpdateCoursePracticeCommand))]
public record UpdateCoursePracticeViewModel : CreateCoursePracticeViewModel
{
    public Guid Id { get; init; }
}

public record SaveCoursePracticeViewModel : CreateCoursePracticeViewModel
{
    /// <summary>
    /// 主键
    /// </summary>
    public Guid? Id { get; init; }
}