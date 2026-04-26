using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HaoKao.LearnProgressService.Application.ViewModels.DailyStudyDuration;


[AutoMapTo(typeof(Domain.Commands.DailyStudyDuration.CreateDailyStudyDurationCommand))]
public class CreateDailyStudyDurationViewModel : IDto
{

    /// <summary>
    /// 产品Id
    /// </summary>
    [DisplayName("产品Id")]
    [Required(ErrorMessage = "{0}不能为空")]
    public Guid ProductId{ get; set; }

    /// <summary>
    /// 对应的科目Id
    /// </summary>
    [DisplayName("对应的科目Id")]
    [Required(ErrorMessage = "{0}不能为空")]
    public Guid SubjectId{ get; set; }

    /// <summary>
    /// 学习时长
    /// </summary>
    [DisplayName("学习时长")]
    public decimal StudyDuration { get; set; } = 0;

}