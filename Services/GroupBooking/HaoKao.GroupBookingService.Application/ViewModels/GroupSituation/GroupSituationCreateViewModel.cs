using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace HaoKao.GroupBookingService.Application.ViewModels.GroupSituation;

[AutoMapTo(typeof(Domain.Commands.GroupSituation.CreateGroupSituationCommand))]
public class CreateGroupSituationViewModel : IDto
{
    /// <summary>
    /// 拼团资料Id
    /// </summary>
    [DisplayName("拼团资料Id")]
    [Required(ErrorMessage = "{0}不能为空")]
    public Guid GroupDataId { get; set; }

    /// <summary>
    /// 资料名
    /// </summary>
    [DisplayName("资料名")]
    [Required(ErrorMessage = "{0}不能为空")]
    public string DataName { get; set; }

    /// <summary>
    /// 适用科目
    /// </summary>
    public List<Guid> SuitableSubjects { get; set; }

    /// <summary>
    /// 成团人数
    /// </summary>
    [DisplayName("成团人数")]
    [Required(ErrorMessage = "{0}不能为空")]
    public int PeopleNumber { get; set; }

    /// <summary>
    /// 限制时间
    /// </summary>
    [DisplayName("限制时间")]
    [Required(ErrorMessage = "{0}不能为空")]
    public int LimitTime { get; set; }

    /// <summary>
    /// 拼团资料
    /// </summary>
    [DisplayName("拼团资料")]
    public string Document { get; set; }


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

//[AutoMapTo(typeof(GroupMember))]
//public class CreateGroupMemberViewModel : IDto
//{
//    /// <summary>
//    /// 用户Id
//    /// </summary>
//    [DisplayName("用户Id")]
//    public Guid UserId { get; set; }

//    /// <summary>
//    /// 昵称
//    /// </summary>
//    [DisplayName("昵称")]
//    [Required(ErrorMessage = "{0}不能为空")]
//    public string Name { get; set; }

//    /// <summary>
//    /// 用户头像Url
//    /// </summary>
//    [DisplayName("用户头像Url")]
//    [Required(ErrorMessage = "{0}不能为空")]
//    public string ImageUrl { get; set; }
//}