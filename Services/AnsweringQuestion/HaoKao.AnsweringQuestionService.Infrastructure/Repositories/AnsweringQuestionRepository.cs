using HaoKao.AnsweringQuestionService.Domain.Repositories;
using System.Threading.Tasks;
using System;
using Girvs.Infrastructure;
using System.Data;
using System.Linq;
using Girvs.BusinessBasis.Queries;
using System.Collections.Generic;
using Girvs.Extensions.Collections;
using HaoKao.AnsweringQuestionService.Domain.Entities;

namespace HaoKao.AnsweringQuestionService.Infrastructure.Repositories;

public class AnsweringQuestionRepository : Repository<AnsweringQuestion>, IAnsweringQuestionRepository
{
    public override async Task<List<AnsweringQuestion>> GetByQueryAsync(QueryBase<AnsweringQuestion> query)
    {
        query.RecordCount = await Queryable.Where(query.GetQueryWhere()).Where(x => x.ParentId == null).CountAsync();

        if (query.RecordCount < 1)
        {
            query.Result = [];
        }
        else
        {
            query.Result =
                await Queryable.Where(query.GetQueryWhere()).Where(x => x.ParentId == null)
                               .SelectProperties(query.QueryFields)
                               .OrderByDescending(x => x.WatchCount)
                               .ThenByDescending(x => x.CreateTime)
                               .Skip(query.PageStart)
                               .Take(query.PageSize).ToListAsync();
        }

        return query.Result;
    }

    public async Task<int> GetCountByProductId(Guid productId, Guid userId)
    {
        userId = EngineContext.Current.ClaimManager.IdentityClaim.GetUserId<Guid>();
        var count = await Queryable.CountAsync(x => x.ProductId == productId & x.CreatorId == userId);
        return count;
    }

    /// <summary>
    /// 查询当前答疑是否存在追问
    /// </summary>
    /// <param name="answerQuestionId"></param>
    /// <returns></returns>
    public async Task<AnsweringQuestion> GetChildQuestion(Guid answerQuestionId)
    {
        var result = await Queryable.FirstOrDefaultAsync(x => x.ParentId == answerQuestionId);
        return result;
    }

    public override async Task<AnsweringQuestion> GetByIdAsync(Guid id)
    {
        var result = await Queryable.Include(x => x.ChildQuestion)
                                    .Include(x => x.AnsweringQuestionReplys)
                                    .FirstOrDefaultAsync(x => x.Id == id);
        return result;
    }

    public async Task<int> UpdateIsReply(Guid id)
    {
        var tableName = EngineContext.Current.GetEntityShardingTableParameter<AnsweringQuestion>().GetCurrentShardingTableName();
        var sql = $"update {tableName} set {nameof(AnsweringQuestion.IsReply)}=1 where id='{id}'";
        return await ExecuteNonQueryAsync(sql);
    }

    public async Task<int> DeleteChildQuestion(Guid id)
    {
        var tableName = EngineContext.Current.GetEntityShardingTableParameter<AnsweringQuestion>().GetCurrentShardingTableName();
        var sql = $"delete from {tableName} where ParentId='{id}'";
        return await ExecuteNonQueryAsync(sql);
    }

    public async Task<int> DeleteQuestion(Guid id)
    {
        var tableName = EngineContext.Current.GetEntityShardingTableParameter<AnsweringQuestion>().GetCurrentShardingTableName();
        var sql = $"delete from {tableName} where Id='{id}'";
        return await ExecuteNonQueryAsync(sql);
    }

    public static async Task<int> ExecuteNonQueryAsync(string sql)
    {
        var context = EngineContext.Current.Resolve<AnsweringQuestionDbContext>();
        var connection = context.Database.GetDbConnection();
        await context.Database.OpenConnectionAsync();
        var command = connection.CreateCommand();
        command.CommandText = sql;
        command.CommandType = CommandType.Text;
        var rows = await command.ExecuteNonQueryAsync();
        await context.Database.CloseConnectionAsync();
        return rows;
    }
}