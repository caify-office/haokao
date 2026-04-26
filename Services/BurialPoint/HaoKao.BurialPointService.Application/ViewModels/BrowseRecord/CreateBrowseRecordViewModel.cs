using HaoKao.BurialPointService.Domain.Enums;
using System.Text.Json.Serialization;

namespace HaoKao.BurialPointService.Application.ViewModels.BrowseRecord;


[AutoMapTo(typeof(Domain.Entities.BrowseRecord))]
public class CreateBrowseRecordViewModel : IDto
{
    /// <summary>
    /// 埋点名称
    /// </summary>
    [DisplayName("埋点名称")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MinLength(2, ErrorMessage = "{0}长度不能小于{1}")]
    [MaxLength(50, ErrorMessage = "{0}长度不能大于{1}")]
    public string BurialPointName { get; set; }

    /// <summary>
    /// 所属端
    /// </summary>
    [JsonIgnore]
    public BelongingPortType BelongingPortType { get; set; }

    /// <summary>
    /// 浏览信息
    /// </summary>
    [DisplayName("浏览信息")]
    [Required(ErrorMessage = "{0}不能为空")]
    public string BrowseData { get; set; }

    /// <summary>
    /// 是否付费用户
    /// </summary>
    [DisplayName("是否付费用户")]
    [Required(ErrorMessage = "{0}不能为空")]
    public bool IsPaidUser { get; set; }
}