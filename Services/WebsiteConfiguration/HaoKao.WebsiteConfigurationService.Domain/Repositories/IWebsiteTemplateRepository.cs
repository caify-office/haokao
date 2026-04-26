using HaoKao.WebsiteConfigurationService.Domain.Enumerations;
using System.Collections.Generic;

namespace HaoKao.WebsiteConfigurationService.Domain.Repositories;

public interface IWebsiteTemplateRepository : IRepository<WebsiteTemplate>
{
    /// <summary>
    /// 获取当前域名，当且英文名下的模板
    /// </summary>
    /// <param name="domainName"></param>
    /// <param name="englishName"></param>
    /// <returns></returns>
    Task<List<Tuple<Guid, string, WebsiteTemplateType, string, Guid, string, bool>>> GetWebsiteTemplateByDominNameAneEnglishName(string domainName, string englishName);
    /// <summary>
    /// 根据域名获取符合条件得模板
    /// </summary>
    /// <param name="domainName"></param>
    /// <returns></returns>

    Task<List<WebsiteTemplate>> GetByDomainNameAsync(string domainName);

}
