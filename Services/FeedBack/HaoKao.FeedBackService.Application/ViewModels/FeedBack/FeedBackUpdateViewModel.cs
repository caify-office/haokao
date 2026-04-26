using HaoKao.FeedBackService.Domain.Enums;

namespace HaoKao.FeedBackService.Application.ViewModels.FeedBack;

[AutoMapTo(typeof(Domain.Entities.FeedBack))]
public class UpdateFeedBackViewModel : IDto
{
    /// <summary>
    /// 状态
    /// </summary>
    [DisplayName("状态")]
    [Required(ErrorMessage = "{0}不能为空")]
    public StatusEnum Status { get; set; }
}