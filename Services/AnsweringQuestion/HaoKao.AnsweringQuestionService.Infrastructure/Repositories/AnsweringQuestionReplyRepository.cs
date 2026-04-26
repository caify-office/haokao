using HaoKao.AnsweringQuestionService.Domain.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;
using Girvs.BusinessBasis.Queries;
using Girvs.Extensions.Collections;
using HaoKao.AnsweringQuestionService.Domain.Entities;

namespace HaoKao.AnsweringQuestionService.Infrastructure.Repositories;

public class AnsweringQuestionReplyRepository : Repository<AnsweringQuestionReply>, IAnsweringQuestionReplyRepository
{

    public async Task<List<AnsweringQuestionReply>> GetAnsweringQuestionReplyList(Guid AnsweringQuestionId)
    {
        return await Queryable.Where(predicate => predicate.AnsweringQuestionId == AnsweringQuestionId).OrderByDescending(x=>x.CreateTime).ToListAsync();
    }

    public async Task<int> GetAnsweringQuestionReplyCount(Guid AnsweringQuestionId)
    {
        return await Queryable.CountAsync(predicate => predicate.AnsweringQuestionId == AnsweringQuestionId);
    }

    public override async Task<List<AnsweringQuestionReply>> GetByQueryAsync(QueryBase<AnsweringQuestionReply> query)
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
