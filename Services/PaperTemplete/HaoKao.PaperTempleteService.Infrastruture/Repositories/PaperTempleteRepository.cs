using Girvs.BusinessBasis.Queries;
using Girvs.Extensions;
using Girvs.Extensions.Collections;
using HaoKao.PaperTempleteService.Domain.Entities;
using HaoKao.PaperTempleteService.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace HaoKao.PaperTempleteService.Infrastructure.Repositories;

public class PaperTempleteRepository : Repository<PaperTemplete>, IPaperTempleteRepository
{
    public async Task<List<PaperTemplete>> GetPaperTempleteByQueryAsync(string subjectId)
    {
        Expression<Func<PaperTemplete, bool>> expression = OtherQueryCondition;
        var resultTree = new List<PaperTemplete>();

        if (!string.IsNullOrEmpty(subjectId))
        {
            expression = expression.And(x => x.SuitableSubjects.Contains(subjectId));
        }

        resultTree = await Queryable.Where(expression).ToListAsync();
        return resultTree;
    }

    public override async Task<List<PaperTemplete>> GetByQueryAsync(QueryBase<PaperTemplete> query)
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
                               .OrderByDescending(x => x.CreateTime)
                               .Skip(query.PageStart)
                               .Take(query.PageSize).ToListAsync();
        }

        return query.Result;
    }
}