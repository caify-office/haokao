using HaoKao.CampaignService.Domain.Commands;

namespace HaoKao.CampaignService.Application.ViewModels.GiftBag;

[AutoMapTo(typeof(UpdateGiftBagCommand))]
public record UpdateGiftBagViewModel : CreateGiftBagViewModel
{
    /// <summary>
    /// Id
    /// </summary>
    [DisplayName("Id")]
    [Required(ErrorMessage = "{0}不能为空")]
    public Guid Id { get; init; }
}

[AutoMapTo(typeof(UpdateGiftBagPublishedCommand))]
public record UpdateGiftBagPublishedViewModel : IDto
{
    /// <summary>
    /// Ids
    /// </summary>
    [DisplayName("Ids")]
    [Required(ErrorMessage = "{0}不能为空")]
    public IReadOnlyList<Guid> Ids { get; init; }

    /// <summary>
    /// 是否发布
    /// </summary>
    [DisplayName("是否发布")]
    [Required(ErrorMessage = "{0}不能为空")]
    public bool IsPublished { get; init; }
}