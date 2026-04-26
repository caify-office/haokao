namespace HaoKao.CorrectionNotebookService.Application.ViewModels;

public record ExamLevelListItemViewModel(Guid Id, string Name, bool IsBuiltIn, DateTime CreateTime, IReadOnlyList<ExamLevelListItemChild> Children) : IDto;

public record ExamLevelListItemChild(Guid Id, string Name, bool IsBuiltIn, Guid ParentId, DateTime CreateTime) : IDto;

public record CreateExamLevelViewModel(string Name, Guid ParentId) : IDto;

public record EditExamLevelNameViewModel(Guid Id, string Name) : IDto;