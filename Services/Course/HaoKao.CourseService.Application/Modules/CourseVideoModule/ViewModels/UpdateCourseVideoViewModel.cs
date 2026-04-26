using HaoKao.CourseService.Domain.CourseVideoModule;

namespace HaoKao.CourseService.Application.Modules.CourseVideoModule.ViewModels;

[AutoMapTo(typeof(UpdateCourseVideoCommand))]
public record UpdateCourseVideoViewModel : SaveCourseVideoViewModel
{
    /// <summary>
    /// Id
    /// </summary>
    public Guid Id { get; init; }
}

public record BatchUpdateNameViewModel(List<Guid> Ids, string Name) : IDto;

public record UpdateCourseVideoNewViewModel(string CreateTime, List<CourseInfo> Courses) : IDto;

public record CourseInfo(string CourseName, List<string> VideoNames);

public record UpdateKnowledgePointModel(Guid Id, string KnowledgepointIds) : IDto;

public record UpdateSortModel(Guid Id, int Sort) : IDto;