using HaoKao.AgreementService.Domain.Commands;

namespace HaoKao.AgreementService.Application.ViewModels.CourseAgreement;

[AutoMapTo(typeof(UpdateCourseAgreementCommand))]
public record UpdateCourseAgreementViewModel : CreateCourseAgreementViewModel
{
    /// <summary>
    /// 主键Id
    /// </summary>
    [JsonIgnore]
    public Guid Id { get; set; }
}