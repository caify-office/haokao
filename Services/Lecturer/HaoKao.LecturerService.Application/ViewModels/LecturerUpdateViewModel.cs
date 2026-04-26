using HaoKao.LecturerService.Domain.Commands;

namespace HaoKao.LecturerService.Application.ViewModels;

[AutoMapTo(typeof(UpdateLecturerCommand))]
public record UpdateLecturerViewModel : CreateLecturerViewModel
{
    [DisplayName("教师Id")]
    [Required(ErrorMessage = "{0}不能为空")]
    public Guid Id { get; init; }
}