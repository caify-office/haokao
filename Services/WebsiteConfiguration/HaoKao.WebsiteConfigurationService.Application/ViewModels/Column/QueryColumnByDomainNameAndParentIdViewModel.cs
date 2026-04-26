namespace HaoKao.WebsiteConfigurationService.Application.ViewModels.Column;


[AutoMapTo(typeof(Domain.Models.Column))]
public class QueryColumnByDomainNameAndParentIdViewModel : IDto
{
    /// <summary>
    /// 域名
    /// </summary>
    [DisplayName("域名")]
    public string DomainName { get; set; }
   

    /// <summary>
    /// 父节点id
    /// </summary>
    [DisplayName("父节点id")]
    public Guid? ParentId { get; set; }

  
}