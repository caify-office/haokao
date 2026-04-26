using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HaoKao.GroupBookingService.Application.ViewModels.GroupData;

[AutoMapTo(typeof(Domain.Commands.GroupData.UpdateGroupDataCommand))]
public class UpdateGroupDataViewModel : IDto
{
    public Guid Id { get; set; }
    /// <summary>
    /// 资料名
    /// </summary>
    [DisplayName("资料名")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MinLength(2, ErrorMessage = "{0}长度不能小于{1}")]
    [MaxLength(50, ErrorMessage = "{0}长度不能大于{1}")]
    public string DataName { get; set; }

    /// <summary>
    /// 适用科目
    /// </summary>
    [DisplayName("适用科目")]
    public List<Guid> SuitableSubjects { get; set; }

    /// <summary>
    /// 成团人数
    /// </summary>
    [DisplayName("成团人数")]
    [Required(ErrorMessage = "{0}不能为空")]
    public int PeopleNumber { get; set; }

    /// <summary>
    /// 基础拼团成功人数
    /// </summary>
    [DisplayName("基础拼团成功人数")]
    public int BasePeopleNumber { get; set; }

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
    /// 状态
    /// </summary>
    [DisplayName("状态")]
    [Required(ErrorMessage = "{0}不能为空")]
    public bool State { get; set; }
   
}