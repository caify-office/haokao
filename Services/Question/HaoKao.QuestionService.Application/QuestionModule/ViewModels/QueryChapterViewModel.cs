using HaoKao.QuestionService.Domain.QuestionModule;

namespace HaoKao.QuestionService.Application.QuestionModule.ViewModels;

public record BaseQueryChapterListViewModel : IDto
{
    /// <summary>
    /// 类别Id
    /// </summary>
    public Guid QuestionCategoryId { get; init; }

    /// <summary>
    /// 是否免费
    /// </summary>
    public FreeState? FreeState { get; init; }
}

public record QueryChapterViewModel(Guid SubjectId) : BaseQueryChapterListViewModel;

public record QueryChapterQuestionViewModel(Guid ChapterId) : BaseQueryChapterListViewModel;

public record QuerySectionViewModel(Guid ChapterId) : BaseQueryChapterListViewModel;

public record QuerySectionQuestionViewModel(Guid SectionId) : BaseQueryChapterListViewModel;

public record QueryKnowledgePointViewModel(Guid SectionId) : BaseQueryChapterListViewModel;

public record QueryKnowledgePointQuestionViewModel(Guid KnowledgePointId) : BaseQueryChapterListViewModel;

public record ChapterViewModel(Guid Id, string Name, int Count) : IDto;

public record QuerySubjectQuestionCountViewModel(Guid SubjectId, IReadOnlyList<Guid> QuestionCategoryIds) : IDto;