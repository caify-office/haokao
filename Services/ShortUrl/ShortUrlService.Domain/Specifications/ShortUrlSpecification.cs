using ShortUrlService.Domain.Entities;
using ShortUrlService.Domain.Specifications.Base;

namespace ShortUrlService.Domain.Specifications;

public sealed class ShortUrlSpecification : BaseSpecification<ShortUrl>
{
    public ShortUrlSpecification(long registerAppId, string originUrl)
    {
        AddCriteria(x => x.RegisterAppId == registerAppId && x.OriginUrl == originUrl && !x.IsDelete);
    }
}

public sealed class ShortUrlByShortKeySpec : BaseSpecification<ShortUrl>
{
    public ShortUrlByShortKeySpec(string shortKey)
    {
        AddCriteria(x => x.ShortKey == shortKey && !x.IsDelete);
    }
}

public sealed class ShortUrlWithAccessLogSpec : BaseSpecification<ShortUrl>
{
    public ShortUrlWithAccessLogSpec(long id) : base(x => x.Id == id)
    {
        AddInclude(x => x.AccessLogs);
    }

    public ShortUrlWithAccessLogSpec(IEnumerable<long> ids) : base(x => ids.Contains(x.Id))
    {
        AddInclude(x => x.AccessLogs);
    }
}