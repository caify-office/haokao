using HaoKao.CourseService.Domain.CourseMaterialsModule;

namespace HaoKao.CourseService.Application.Modules.CourseMaterialsModule.ViewModels;

[AutoMapFrom(typeof(CourseMaterialsQuery))]
[AutoMapTo(typeof(CourseMaterialsQuery))]
public class QueryCourseMaterialsViewModel : QueryDtoBase<BrowseCourseMaterialsViewModel>
{
    /// <summary>
    /// 关联的知识点id
    /// </summary>
    public Guid? KnowledgePointId { get; set; }

    /// <summary>
    /// 关联的课程章节id（阶段学习为课程章节id,智慧辅助学习为课程Id）
    /// </summary>
    public Guid? CourseChapterId { get; set; }

    /// <summary>
    /// 课程id（阶段学习专用）
    /// </summary>
    public Guid? CourseId { get; set; }
}