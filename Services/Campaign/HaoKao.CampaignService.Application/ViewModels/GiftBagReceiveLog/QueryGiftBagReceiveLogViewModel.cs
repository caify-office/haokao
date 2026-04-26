using HaoKao.CampaignService.Domain.Queries;

namespace HaoKao.CampaignService.Application.ViewModels.GiftBagReceiveLog;

[AutoMapTo(typeof(GiftBagReceiveLogQuery))]
[AutoMapFrom(typeof(GiftBagReceiveLogQuery))]
public class QueryGiftBagReceiveLogViewModel : QueryDtoBase<BrowseGiftBagReceiveLogViewModel>
{
    [Required]
    public Guid GiftBagId { get; init; }

    /// <summary>
    /// 用户名
    /// </summary>
    public string ReceiverName { get; init; }

    /// <summary>
    /// 领取时间-开始
    /// </summary>
    public DateTime? StartTime { get; init; }

    /// <summary>
    /// 领取时间-结束
    /// </summary>
    public DateTime? EndTime { get; set; }
}

[AutoMapTo(typeof(Domain.Entities.GiftBagReceiveLog))]
[AutoMapFrom(typeof(Domain.Entities.GiftBagReceiveLog))]
public record BrowseGiftBagReceiveLogViewModel : IDto
{
    /// <summary>
    /// 领取人Id
    /// </summary>
    public Guid ReceiverId { get; init; }

    /// <summary>
    /// 领取人名称
    /// </summary>
    public string ReceiverName { get; init; }

    /// <summary>
    /// 领取时间
    /// </summary>
    public DateTime ReceiveTime { get; init; }

    /// <summary>
    /// 注册时间
    /// </summary>
    public DateTime RegistrationTime { get; init; }
}