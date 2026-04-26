using Girvs.Infrastructure;
using HaoKao.NoticeService.Domain.Models;
using HaoKao.NoticeService.Domain.Repositories;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace HaoKao.NoticeService.Infrastructure.Repositories;

public class NoticeRepository : Repository<Notice>, INoticeRepository
{
    public IQueryable<Notice> Query => Queryable.AsNoTracking();

    public async Task<List<Notice>> GetPublishedNoticeList(QueryBase<Notice> noticeQuery)
    {
        var predicate = noticeQuery.GetQueryWhere();
        var query = Query.Where(predicate).OrderByDescending(x => x.CreateTime);
        var sql = query.ToQueryString();
        var suffix = EngineContext.Current.GetEntityShardingTableParameter<Notice>().GetCurrentShrdingTableSuffix();
        if (!string.IsNullOrEmpty(suffix))
        {
            sql = sql.Replace(suffix, "");
        }

        // 查询平台级别的公告
        var notices = await EngineContext.Current.Resolve<NoticeDbContext>().Notices.FromSqlRaw(sql).ToListAsync();

        // 查询租户级别的公告
        if (EngineContext.Current.HttpContext.Request.Headers["TenantId"] != Guid.Empty.ToString())
        {
            notices.AddRange(await query.ToListAsync());
        }

        return notices.Distinct().ToList();
    }

    public Task<int> ExecuteUpdateAsync(Expression<Func<Notice, bool>> predicate, Expression<Func<SetPropertyCalls<Notice>, SetPropertyCalls<Notice>>> setPropertyCalls)
    {
        return Queryable.Where(predicate).ExecuteUpdateAsync(setPropertyCalls);
    }
}