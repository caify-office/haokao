using HaoKao.StudyMaterialService.Domain.Commands;

namespace HaoKao.StudyMaterialService.Application.ViewModels;

[AutoMapTo(typeof(EnableStudyMaterialCommand))]
public record EnableStudyMaterialViewModel : IDto
{
    /// <summary>
    /// Ids
    /// </summary>
    [DisplayName("Ids")]
    [Required(ErrorMessage = "{0}不能为空")]
    public IReadOnlyList<Guid> Ids { get; init; }

    /// <summary>
    /// 启用/禁用
    /// </summary>
    [DisplayName("启用/禁用")]
    [Required(ErrorMessage = "{0}不能为空")]
    public bool Enable { get; init; }
}