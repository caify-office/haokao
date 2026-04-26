namespace HaoKao.WebsiteConfigurationService.Application.ViewModels.TemplateStyle;


[AutoMapFrom(typeof(Domain.Models.TemplateStyle))]
public class BrowseTemplateStyleViewModel : IDto
{

    /// <summary>
    /// 域名
    /// </summary>
    public string DomainName { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 路径
    /// </summary>
    public string Path { get; set; }
}
