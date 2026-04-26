namespace HaoKao.CorrectionNotebookService.Application.ViewModels;

/// <summary>
/// 标签列表项
/// </summary>
/// <param name="Category"></param>
/// <param name="IsBuiltIn"></param>
/// <param name="CreateTime"></param>
/// <param name="Tags"></param>
public record TagCategoryViewModel(string Category, bool IsBuiltIn, DateTime CreateTime, IReadOnlyList<TagCategoryItemViewModel> Tags) : IDto;

/// <summary>
/// 标签列表项
/// </summary>
/// <param name="Id"></param>
/// <param name="Name">名称</param>
/// <param name="IsBuiltIn">是否内置数据</param>
/// <param name="CreateTime"></param>
public record TagCategoryItemViewModel(Guid Id, string Name, bool IsBuiltIn, DateTime CreateTime) : IDto;

public record CreateTagViewModel(string Name) : IDto;