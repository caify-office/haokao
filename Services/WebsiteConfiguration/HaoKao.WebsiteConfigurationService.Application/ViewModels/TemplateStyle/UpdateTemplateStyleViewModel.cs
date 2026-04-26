namespace HaoKao.WebsiteConfigurationService.Application.ViewModels.TemplateStyle;


[AutoMapTo(typeof(Domain.Models.TemplateStyle))]
public class UpdateTemplateStyleViewModel : IDto
{
    public Guid Id { get; set; }
    /// <summary>
    /// 域名
    /// </summary>
    [DisplayName("域名")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MinLength(2, ErrorMessage = "{0}长度不能小于{1}")]
    [MaxLength(500, ErrorMessage = "{0}长度不能大于{1}")]
    public string DomainName { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    [DisplayName("名称")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MinLength(2, ErrorMessage = "{0}长度不能小于{1}")]
    [MaxLength(50, ErrorMessage = "{0}长度不能大于{1}")]
    public string Name { get; set; }

    /// <summary>
    /// 路径
    /// </summary>
    [DisplayName("路径")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MinLength(2, ErrorMessage = "{0}长度不能小于{1}")]
    [MaxLength(500, ErrorMessage = "{0}长度不能大于{1}")]
    public string Path { get; set; }
}