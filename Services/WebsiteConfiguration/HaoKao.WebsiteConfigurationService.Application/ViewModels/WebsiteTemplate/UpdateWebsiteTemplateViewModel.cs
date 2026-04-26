using HaoKao.WebsiteConfigurationService.Domain.Enumerations;

namespace HaoKao.WebsiteConfigurationService.Application.ViewModels.WebsiteTemplate;


[AutoMapTo(typeof(Domain.Commands.WebsiteTemplate.UpdateWebsiteTemplateCommand))]
public class UpdateWebsiteTemplateViewModel : IDto
{

    /// <summary>
    /// 名称
    /// </summary>
    [DisplayName("名称")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MinLength(2, ErrorMessage = "{0}长度不能小于{1}")]
    [MaxLength(50, ErrorMessage = "{0}长度不能大于{1}")]
    public string Name { get; set; }

    /// <summary>
    /// 网站模板类型
    /// </summary>
    [DisplayName("网站模板类型")]
    [Required(ErrorMessage = "{0}不能为空")]
    public WebsiteTemplateType WebsiteTemplateType { get; set; }


    /// <summary>
    /// 描述
    /// </summary>
    [DisplayName("描述")]
    public string Desc { get; set; }

    /// <summary>
    /// 所属栏目Id
    /// </summary>
    [DisplayName("所属栏目Id")]
    [Required(ErrorMessage = "{0}不能为空")]
    public Guid ColumnId { get; set; }

    /// <summary>
    /// 所属栏目名称
    /// </summary>
    [DisplayName("所属栏目名称")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MinLength(2, ErrorMessage = "{0}长度不能小于{1}")]
    [MaxLength(50, ErrorMessage = "{0}长度不能大于{1}")]
    public string ColumnName { get; set; }


    /// <summary>
    /// 是否默认
    /// </summary>
    [DisplayName("是否默认")]
    [Required(ErrorMessage = "{0}不能为空")]
    public bool IsDefault { get; set; }
    public Guid Id { get; internal set; }
}