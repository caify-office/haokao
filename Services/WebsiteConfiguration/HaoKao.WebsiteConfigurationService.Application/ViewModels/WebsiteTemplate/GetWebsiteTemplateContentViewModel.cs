namespace HaoKao.WebsiteConfigurationService.Application.ViewModels.WebsiteTemplate;


public class GetWebsiteTemplateContentViewModel : IDto
{

    /// <summary>
    /// 域名
    /// </summary>
    public string DomainName { get; set; }
    /// <summary>
    /// 英文名
    /// </summary>
    public string EnglishName { get; set; }

}