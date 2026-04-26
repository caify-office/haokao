namespace HaoKao.CorrectionNotebookService.Application.ViewModels;

public record SubjectListItemViewModel(Guid Id, string Name, int QuestionCount, bool IsBuiltIn, int Sort, string Icon) : IDto;

public record CreateSubjectViewModel(Guid ExamLevelId, string Name) : IDto;

public record ResortSubjectViewModel(Guid ExamLevelId, Dictionary<Guid, int> Dict) : IDto;