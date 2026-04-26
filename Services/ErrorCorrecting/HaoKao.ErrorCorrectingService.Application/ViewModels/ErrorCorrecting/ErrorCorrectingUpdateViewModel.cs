using HaoKao.ErrorCorrectingService.Domain.Entities;
using System.ComponentModel;

namespace HaoKao.ErrorCorrectingService.Application.ViewModels.ErrorCorrecting;

[AutoMapTo(typeof(Domain.Entities.ErrorCorrecting))]
public class UpdateErrorCorrectingViewModel : IDto
{
    [DisplayName("状态")]
    public StatusEnum Status { get; set; } //处理状态
}