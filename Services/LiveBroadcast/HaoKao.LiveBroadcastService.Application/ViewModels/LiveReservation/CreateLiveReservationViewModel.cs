using HaoKao.LiveBroadcastService.Domain.Commands.LiveReservation;
using HaoKao.LiveBroadcastService.Domain.Enums;
using System.Text.Json.Serialization;

namespace HaoKao.LiveBroadcastService.Application.ViewModels.LiveReservation;

[AutoMapTo(typeof(CreateLiveReservationCommand))]
public class CreateLiveReservationViewModel : IDto
{
    /// <summary>
    /// 预约直播产品id
    /// </summary>
    [DisplayName("预约直播产品id")]
    [Required(ErrorMessage = "{0}不能为空")]
    public Guid ProductId { get; set; }

    /// <summary>
    /// 预约视频直播Id
    /// </summary>
    [DisplayName(" 预约视频直播Id")]
    [Required(ErrorMessage = "{0}不能为空")]
    public Guid LiveVideoId { get; set; }

    /// <summary>
    /// 手机号
    /// </summary>
    [DisplayName("手机号")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MinLength(2, ErrorMessage = "{0}长度不能小于{1}")]
    [MaxLength(50, ErrorMessage = "{0}长度不能大于{1}")]
    public string Phone { get; set; }

    /// <summary>
    /// 预约来源
    /// </summary>
    [DisplayName("预约来源")]
    [JsonIgnore]
    public ReservationSource ReservationSource { get; set; }

    /// <summary>
    /// OpenId
    /// </summary>
    [DisplayName("OpenId")]
    public string OpenId { get; set; }
}