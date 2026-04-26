using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace HaoKao.GroupBookingService.Application.ViewModels.GroupSituation;

/// <summary>
/// 加入拼团
/// </summary>
[AutoMapTo(typeof(Domain.Commands.GroupSituation.JoinGroupSituationCommand))]
public class GroupMemberJoinViewModel : IDto
{
    /// <summary>
    /// 拼团资料Id
    /// </summary>
    [DisplayName("拼团资料Id")]
    [Required(ErrorMessage = "{0}不能为空")]
    public Guid GroupDataId { get; set; }

    /// <summary>
    /// 拼团Id
    /// </summary>
    [DisplayName("拼团Id")]
    [Required(ErrorMessage = "{0}不能为空")]
    public Guid GroupSituationId { get; set; }



    /// <summary>
    /// 昵称
    /// </summary>
    [DisplayName("昵称")]
    [Required(ErrorMessage = "{0}不能为空")]
    public string Name { get; set; }

    /// <summary>
    /// 用户头像Url
    /// </summary>
    [DisplayName("用户头像Url")]
    public string ImageUrl { get; set; }
}