using HaoKao.WebsiteConfigurationService.Domain.Enumerations;

namespace HaoKao.WebsiteConfigurationService.Application.ViewModels.WebsiteTemplate;


[AutoMapFrom(typeof(WebsiteTemplateQuery))]
[AutoMapTo(typeof(WebsiteTemplateQuery))]
public class WebsiteTemplateQueryViewModel : QueryDtoBase<WebsiteTemplateQueryListViewModel>
{
    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 内容
    /// </summary>
    public string Content { get; set; }

    /// <summary>
    /// 所属栏目Id
    /// </summary>
    public Guid? ColumnId { get; set; }

    /// <summary>
    /// 所属栏目名称
    /// </summary>
    public string ColumnName { get; set; }

    /// <summary>
    /// 是否启用
    /// </summary>
    public bool? IsEnable { get; set; }

    /// <summary>
    /// 是否默认
    /// </summary>
    public bool? IsDefault { get; set; }
}

[AutoMapFrom(typeof(Domain.Models.WebsiteTemplate))]
[AutoMapTo(typeof(Domain.Models.WebsiteTemplate))]
public class WebsiteTemplateQueryListViewModel : IDto
{
    public Guid Id { get; set; }
    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 模板类型
    /// </summary>
    public WebsiteTemplateType WebsiteTemplateType { get; set; }

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



    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }
}


[AutoMapFrom(typeof(Domain.Models.WebsiteTemplate))]
[AutoMapTo(typeof(Domain.Models.WebsiteTemplate))]
public class WebsiteTemplateQueryWebListViewModel : IDto
{
    public Guid Id { get; set; }
    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 模板类型
    /// </summary>
    public WebsiteTemplateType WebsiteTemplateType { get; set; }

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