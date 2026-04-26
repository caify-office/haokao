using Girvs.BusinessBasis.Entities;

namespace ShortUrlService.Domain.Entities;

public class AccessLog : AggregateRoot<long>, IIncludeCreateTime
{
    public AccessLog() { }

    public AccessLog(long id, long shortUrlId, string ip, int osType, int browserType, string userAgent)
    {
        Id = id;
        ShortUrlId = shortUrlId;
        Ip = ip;
        OsType = osType;
        BrowserType = browserType;
        UserAgent = userAgent;
        CreateTime = DateTime.Now;
    }

    /// <summary>
    /// 短链接Id
    /// </summary>
    public long ShortUrlId { get; set; }

    /// <summary>
    /// IP地址
    /// </summary>
    public string Ip { get; set; } = null!;

    /// <summary>
    /// 系统类型
    /// </summary>
    public int OsType { get; set; }

    /// <summary>
    /// 浏览器类型
    /// </summary>
    public int BrowserType { get; set; }

    /// <summary>
    /// UserAgent
    /// </summary>
    public string UserAgent { get; set; } = null!;

    /// <summary>
    /// 访问时间
    /// </summary>
    public DateTime CreateTime { get; set; }
}