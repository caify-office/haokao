using System.Collections.Generic;

namespace HaoKao.WebsiteConfigurationService.Domain.Repositories;

public interface IColumnRepository : IRepository<Column>
{
    /// <summary>
    /// 根据域名/父Id获取指定栏目的直接子栏目
    /// </summary>
    /// <param name="domainName"></param>
    /// <param name="parentId"></param>
    /// <returns></returns>
    Task<List<ColumnTreeModel>> GetTreeChildren(string domainName, Guid? parentId);
    /// <summary>
    /// 获取当前域名和英文名栏目下属子栏目的信息
    /// </summary>
    /// <param name="domainName"></param>
    /// <param name="englishName"></param>
    /// <returns></returns>
    Task<List<ColumnTreeModel>> GetChildrenColumnByDomainNameAndEnglishName(string domainName, string englishName);
}

public class ColumnTreeModel
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
    /// 是否是主页
    /// </summary>
    public bool IsHomePage { get; set; }

    /// <summary>
    /// 是否叶子节点
    /// </summary>
    public bool IsLeaf { get; set; }
}
