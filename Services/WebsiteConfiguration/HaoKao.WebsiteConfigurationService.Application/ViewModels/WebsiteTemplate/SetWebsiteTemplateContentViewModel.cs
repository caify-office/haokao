namespace HaoKao.WebsiteConfigurationService.Application.ViewModels.WebsiteTemplate;


[AutoMapTo(typeof(Domain.Models.WebsiteTemplate))]
public class SetWebsiteTemplateContentViewModel : IDto
{

    /// <summary>
    /// 名称
    /// </summary>
    [DisplayName("内容")]
    [Required(ErrorMessage = "{0}不能为空")]
    public string Content { get; set; }


}