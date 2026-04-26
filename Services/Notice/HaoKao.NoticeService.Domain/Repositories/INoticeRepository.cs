using HaoKao.NoticeService.Domain.Models;
using Microsoft.EntityFrameworkCore.Query;

namespace HaoKao.NoticeService.Domain.Repositories;

public interface INoticeRepository : IRepository<Notice>
{
    IQueryable<Notice> Query { get; }

    Task<List<Notice>> GetPublishedNoticeList(QueryBase<Notice> noticeQuery);

    Task<int> ExecuteUpdateAsync(Expression<Func<Notice, bool>> predicate, Expression<Func<SetPropertyCalls<Notice>, SetPropertyCalls<Notice>>> setPropertyCalls);
}