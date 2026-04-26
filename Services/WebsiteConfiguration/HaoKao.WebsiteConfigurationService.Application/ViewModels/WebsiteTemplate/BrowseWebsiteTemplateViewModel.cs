using HaoKao.WebsiteConfigurationService.Domain.Enumerations;

namespace HaoKao.WebsiteConfigurationService.Application.ViewModels.WebsiteTemplate;


[AutoMapFrom(typeof(Domain.Models.WebsiteTemplate))]
public class BrowseWebsiteTemplateViewModel : IDto
{

    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 模板类型
    /// </summary>
    public WebsiteTemplateType WebsiteTemplateType { get; set; }

    /// <summary>
    /// 内容
    /// </summary>
    public string Content { get; set; }

    /// <summary>
    /// 描述
    /// </summary>
    public string Desc { get; set; }

    /// <summary>
    /// 所属栏目Id
    /// </summary>
    public Guid ColumnId { get; set; }

    /// <summary>
    /// 所属栏目名称
    /// </summary>
    public string ColumnName { get; set; }



    /// <summary>
    /// 是否默认
    /// </summary>
    public bool IsDefault { get; set; }


}
