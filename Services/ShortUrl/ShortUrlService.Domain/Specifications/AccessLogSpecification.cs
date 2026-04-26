using ShortUrlService.Domain.Entities;
using ShortUrlService.Domain.Specifications.Base;

namespace ShortUrlService.Domain.Specifications;

public sealed class AccessLogSpecification : BaseSpecification<AccessLog>
{
    public AccessLogSpecification(long shortUrlId)
    {
        AddCriteria(x => x.ShortUrlId == shortUrlId);
        AddOrderByDescending(x => x.CreateTime);
        AddOrderBy(x => x.Ip);
        AddPaging(1, 10);
    }
}