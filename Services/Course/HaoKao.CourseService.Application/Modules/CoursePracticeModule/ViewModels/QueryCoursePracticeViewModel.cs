using HaoKao.CourseService.Domain.CoursePracticeModule;

namespace HaoKao.CourseService.Application.Modules.CoursePracticeModule.ViewModels;

[AutoMapFrom(typeof(CoursePracticeQuery))]
[AutoMapTo(typeof(CoursePracticeQuery))]
public class QueryCoursePracticeViewModel : QueryDtoBase<CoursePracticeQueryListViewModel>
{
    /// <summary>
    /// 关联的知识点id
    /// </summary>
    public Guid? KnowledgePointId { get; set; }

    /// <summary>
    /// 关联的课程章节id（阶段学习为课程章节id,智慧辅助学习为课程Id）
    /// </summary>
    public Guid? CourseChapterId { get; set; }
}

[AutoMapFrom(typeof(CoursePractice))]
[AutoMapTo(typeof(CoursePractice))]
public record CoursePracticeQueryListViewModel : BrowseCoursePracticeViewModel;