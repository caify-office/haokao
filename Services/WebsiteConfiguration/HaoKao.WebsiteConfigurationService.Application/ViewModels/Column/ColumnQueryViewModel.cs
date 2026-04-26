namespace HaoKao.WebsiteConfigurationService.Application.ViewModels.Column;


[AutoMapFrom(typeof(ColumnQuery))]
[AutoMapTo(typeof(ColumnQuery))]
public class ColumnQueryViewModel : QueryDtoBase<ColumnQueryListViewModel>
{
    /// <summary>
    /// 域名
    /// </summary>
    public string DomainName { get; set; }
    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }

}

[AutoMapFrom(typeof(Domain.Models.Column))]
[AutoMapTo(typeof(Domain.Models.Column))]
public class ColumnQueryListViewModel : IDto
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
    /// 别名
    /// </summary>
    public string Alias { get; set; }

    /// <summary>
    /// 英文名
    /// </summary>
    public string EnglishName { get; set; }

    /// <summary>
    /// 父节点id
    /// </summary>
    public Guid? ParentId { get; set; }


    /// <summary>
    /// 图标
    /// </summary>
    public string Icon { get; set; }

    /// <summary>
    /// 当前图标
    /// </summary>
    public string ActiveIcon { get; set; }


    /// <summary>
    /// 是否首页
    /// </summary>
    public bool IsHomePage { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; }



    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }
}


[AutoMapFrom(typeof(Domain.Models.Column))]
[AutoMapTo(typeof(Domain.Models.Column))]
public class SimpleColumnQueryListViewModel : IDto
{
    public Guid Id { get; set; }
    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }


    /// <summary>
    /// 英文名
    /// </summary>
    public string EnglishName { get; set; }

    /// <summary>
    /// 图标
    /// </summary>
    public string Icon { get; set; }


    /// <summary>
    /// 当前图标
    /// </summary>
    public string ActiveIcon { get; set; }
}