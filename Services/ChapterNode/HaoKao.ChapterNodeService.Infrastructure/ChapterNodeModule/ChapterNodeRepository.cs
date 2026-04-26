using Girvs.BusinessBasis.Queries;
using Girvs.Extensions.Collections;
using Girvs.Infrastructure;
using HaoKao.ChapterNodeService.Domain.ChapterNodeModule;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaoKao.ChapterNodeService.Infrastructure.ChapterNodeModule;

public class ChapterNodeRepository : Repository<ChapterNode>, IChapterNodeRepository
{
    public override async Task<List<ChapterNode>> GetByQueryAsync(QueryBase<ChapterNode> query)
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

    public async Task<ChapterNode> GetBaseParentChapterNode(Guid id)
    {
        var chapter = Queryable.FirstOrDefault(x => x.Id == id);
        if (chapter.ParentIds != null && chapter.ParentIds.Any())
        {
            var parent = await Queryable.Where(x => x.Id == chapter.ParentIds.FirstOrDefault()).FirstOrDefaultAsync();
            return parent;
        }
        return null;
    }

    public Task<List<ChapterNode>> GetChapterNodeList(Guid subjectId)
    {
        return Queryable.Where(p => p.SubjectId == subjectId).OrderBy(x => x.Sort).ToListAsync();
    }

    public Task<List<ChapterNode>> GetChapterNodeListByParentId(Guid parentId)
    {
        return Queryable.Where(p => p.ParentId == parentId).OrderBy(x => x.Sort).ToListAsync();
    }

    public async Task<List<ChapterNode>> GetChapterNodeKnowledgePointTree(Guid subjectId)
    {
        var tempResult = await Queryable.Include(x => x.Children.OrderBy(x => x.Sort)).Include(x => x.KnowledgePoints.OrderBy(x => x.Sort)).Where(p => p.SubjectId == subjectId).OrderBy(x => x.Sort).ToListAsync();
        var result = tempResult.Where(x => x.ParentId == null).ToList();
        var tenantId = EngineContext.Current.ClaimManager.GetTenantId();
        if (tenantId == "08db449e-307f-410b-890b-bf62eb9f5f0c")
        {
            result = result.Where(x => !x.Name.Contains("全真") && !x.Name.Contains("名师") && !x.Name.Contains("考前")).ToList();
        }
        return result;
    }

    public async Task<Guid[]> GetAllChildrenChapterNodeId(Guid id)
    {
        var tempResult = await Queryable.Include(x => x.Children).Where(p => p.Id == id).ToListAsync();
        var target = tempResult.FirstOrDefault(x => x.Id == id);
        var childrenId = new List<Guid>();
        GetChildrenId(target, ref childrenId);
        return childrenId.ToArray();

        static void GetChildrenId(ChapterNode target, ref List<Guid> childrenId)
        {
            if (target.Children is null) return;
            foreach (var item in target.Children)
            {
                childrenId.Add(item.Id);
                GetChildrenId(item, ref childrenId);
            }
        }
    }

    public Task<Dictionary<Guid, string>> GetChapterDictionary(Guid subjectId)
    {
        return Queryable.Where(p => p.SubjectId == subjectId).OrderBy(x => x.Sort)
                        .Select(x => new { Key = x.Id, Value = x.Code + "：" + x.Name })
                        .ToDictionaryAsync(x => x.Key, x => x.Value);
    }

    public async Task<List<ChapterNodeTreeCacheItem>> GetChapterNodeTreeByQueryAsync(Guid? subjectId, Guid? id)
    {
        var tableName = EngineContext.Current.GetEntityShardingTableParameter<ChapterNode>().GetCurrentShardingTableName();
        var sql = $"""
                   SELECT t.*,
                          CASE WHEN (SELECT COUNT(1) FROM {tableName} c WHERE c.ParentId = t.Id) > 0 
                          THEN FALSE ELSE TRUE END AS IsLeaf
                   FROM {tableName} t
                   {SqlWhere()}
                   ORDER BY t.Sort
                   """;

        var context = EngineContext.Current.Resolve<ChapterNodeDbContext>();
        var connection = context.Database.GetDbConnection();
        await context.Database.OpenConnectionAsync();
        var command = connection.CreateCommand();
        command.CommandText = sql;
        command.CommandType = CommandType.Text;

        var result = new List<ChapterNodeTreeCacheItem>();
        var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            result.Add(new ChapterNodeTreeCacheItem(
                           Id: reader["Id"].ToString(),
                           Code: reader["Code"].ToString(),
                           Name: reader["Name"].ToString(),
                           ParentId: reader["ParentId"].ToString(),
                           ParentName: reader["ParentName"].ToString(),
                           Sort: Convert.ToInt32(reader["Sort"]),
                           IsLeaf: Convert.ToBoolean(reader["IsLeaf"])
                       ));
        }
        await context.Database.CloseConnectionAsync();
        return result;

        StringBuilder SqlWhere()
        {
            var tenantId = EngineContext.Current.ClaimManager.GetTenantId();
            var builder = new StringBuilder($"WHERE t.TenantId = '{tenantId}'");
            if (subjectId.HasValue)
            {
                builder.Append($"\n  AND t.subjectId = '{subjectId}'");
            }
            if (id.HasValue)
            {
                builder.Append($"\n  AND t.ParentId = '{id.Value}'");
            }
            else
            {
                builder.Append("\n  AND t.ParentId IS NULL");
            }
            return builder;
        }
    }
}