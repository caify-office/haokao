namespace HaoKao.WebsiteConfigurationService.Application.ViewModels.TemplateStyle;


[AutoMapFrom(typeof(TemplateStyleQuery))]
[AutoMapTo(typeof(TemplateStyleQuery))]
public class TemplateStyleQueryViewModel : QueryDtoBase<TemplateStyleQueryListViewModel>
{
    /// <summary>
    /// 域名
    /// </summary>
    public string DomainName { get; set; }
}

[AutoMapFrom(typeof(Domain.Models.TemplateStyle))]
[AutoMapTo(typeof(Domain.Models.TemplateStyle))]
public class TemplateStyleQueryListViewModel : IDto
{
    public Guid Id { get; set; }
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

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }
}