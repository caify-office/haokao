using Girvs.BusinessBasis.Queries;
using Girvs.Extensions.Collections;
using HaoKao.PaperService.Domain.Entities;
using HaoKao.PaperService.Domain.Enumerations;
using HaoKao.PaperService.Domain.Repositories;
using Microsoft.EntityFrameworkCore.Query;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace HaoKao.PaperService.Infrastructure.Repositories;

public class PaperRepository : Repository<Paper>, IPaperRepository
{
    public IQueryable<Paper> Query => Queryable.AsNoTracking();

    public Task<List<Paper>> GetPaperList(Guid subjectId, Guid categoryId)
    {
        return Query.Where(p => p.CategoryId == categoryId &&
                                p.SubjectId == subjectId &&
                                p.State == StateEnum.Published)
                    .OrderBy(x => x.Sort).ToListAsync();
    }

    public async Task<string> GetPaperCountAndPaperQuestionCount(Guid subjectId)
    {
        var query = Query.Where(p => p.SubjectId == subjectId && p.State == StateEnum.Published);
        var result = await query.Select(x => new { x.Id, x.QuestionCount }).ToListAsync();
        var count = new { PaperCount = result.Count, PaperQuestionCount = result.Sum(x => x.QuestionCount) };
        return JsonConvert.SerializeObject(count);
    }

    public override async Task<List<Paper>> GetByQueryAsync(QueryBase<Paper> query)
    {
        query.RecordCount = await Queryable.Where(query.GetQueryWhere()).CountAsync();

        if (query.RecordCount < 1)
        {
            query.Result = [];
        }
        else
        {
            query.Result =
                await Queryable.Where(query.GetQueryWhere())
                               .SelectProperties(query.QueryFields)
                               .OrderBy(x => x.Sort)
                               .ThenByDescending(x => x.CreateTime)
                               .Skip(query.PageStart)
                               .Take(query.PageSize).ToListAsync();
        }

        return query.Result;
    }

    public Task<int> ExecuteUpdateAsync(Expression<Func<Paper, bool>> predicate, Expression<Func<SetPropertyCalls<Paper>, SetPropertyCalls<Paper>>> setPropertyCalls)
    {
        return Queryable.Where(predicate).ExecuteUpdateAsync(setPropertyCalls);
    }
}