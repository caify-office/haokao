using HaoKao.CorrectionNotebookService.Domain.Entities;

namespace HaoKao.CorrectionNotebookService.Domain.Dtos;

/// <summary>
/// 题目分页Dto
/// </summary>
/// <param name="Count"></param>
/// <param name="Data"></param>
public record QuestionPagedListDto(int Count, IReadOnlyList<Question> Data);