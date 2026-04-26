using HaoKao.FeedBackService.Domain.Enums;

namespace HaoKao.FeedBackService.Application.ViewModels.FeedBack;

[AutoMapTo(typeof(Domain.Entities.FeedBack))]
public class CreateFeedBackViewModel : IDto
{
    /// <summary>
    /// 父id
    /// </summary>
    [DisplayName("父id")]
    public Guid? ParentId { get; set; }

    /// <summary>
    /// 反馈类型
    /// </summary>
    [DisplayName("反馈类型")]
    [Required(ErrorMessage = "{0}不能为空")]
    public TypeEnum Type { get; set; }

    /// <summary>
    /// 反馈来源
    /// </summary>
    [DisplayName("反馈来源")]
    [Required(ErrorMessage = "{0}不能为空")]
    public SourceTypeEnum SourceType { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    [DisplayName("状态")]
    public StatusEnum Status { get; set; }

    /// <summary>
    /// 联系人
    /// </summary>
    [DisplayName("联系人")]
    [Required(ErrorMessage = "{0}不能为空")]
    public string Contract { get; set; }

    /// <summary>
    /// 描述
    /// </summary>
    [DisplayName("描述")]
    [Required(ErrorMessage = "{0}不能为空")]
    public string Description { get; set; }

    /// <summary>
    /// 图片
    /// </summary>
    [DisplayName("图片")]
    [Required(ErrorMessage = "{0}不能为空")]
    public string FileUrls { get; set; }
}