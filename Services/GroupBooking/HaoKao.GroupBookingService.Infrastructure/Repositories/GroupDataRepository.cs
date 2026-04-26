using Girvs.BusinessBasis.Queries;
using Girvs.Extensions;
using Girvs.Extensions.Collections;
using Girvs.Infrastructure;
using HaoKao.GroupBookingService.Domain.Entities;
using HaoKao.GroupBookingService.Domain.Repositories;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaoKao.GroupBookingService.Infrastructure.Repositories;

public class GroupDataRepository : Repository<GroupData>, IGroupDataRepository
{
    /// <summary>
    /// 查询拼团资料列表
    /// </summary>
    /// <param name="subjectIds"></param>
    /// <param name="takeCount"></param>
    /// <returns></returns>
    public async Task<DataTable> GetGroupDataListBySubjectId(Guid[] subjectIds, int? takeCount)
    {
        var groupDataTableName = EngineContext.Current.GetEntityShardingTableParameter<GroupData>().GetCurrentShardingTableName();
        var groupSituationTableName = EngineContext.Current.GetEntityShardingTableParameter<GroupSituation>().GetCurrentShardingTableName();
        var groupMemberTableName = EngineContext.Current.GetEntityShardingTableParameter<GroupMember>().GetCurrentShardingTableName();
        var tenantId = EngineContext.Current.IsAuthenticated
            ? EngineContext.Current.ClaimManager.GetTenantId().To<Guid>()
            : EngineContext.Current.HttpContext.Request.Headers["TenantId"].To<Guid>();
        var userId = EngineContext.Current.IsAuthenticated
            ? EngineContext.Current.ClaimManager.GetUserId().To<Guid>()
            : Guid.Empty;

        var whereBuilder = new StringBuilder();
        whereBuilder.Append("start");
        subjectIds.ToList().ForEach(x => { whereBuilder.Append($"Or LOCATE('{x}',g.SuitableSubjects )>0 "); });
        whereBuilder.Replace("startOr", "");

        var sqlWhere = new StringBuilder();
        sqlWhere.Append($"where g.TenantId='{tenantId}' and g.State = 1 and ({whereBuilder.ToString()})");

        if (takeCount.HasValue)
        {
            sqlWhere.Append(" limit " + takeCount);
        }

        var sql = $"SELECT g.Id,g.DataName,g.PeopleNumber,g.BasePeopleNumber,( SELECT count(1) FROM {groupSituationTableName} WHERE g.Id = GroupDataId && SuccessTime IS NOT NULL) 'SuccessCount',";
        if (userId != Guid.Empty)
        {
            sql += @$"
               (SELECT GroupSituationId FROM {groupMemberTableName} WHERE g.Id = GroupDataId and CreatorId= '{userId}' and SuccessTime is NOT NULL limit 1) 'SuccessGroupSituationId',
               (SELECT GroupSituationId FROM {groupMemberTableName} WHERE g.Id = GroupDataId and CreatorId= '{userId}' and SuccessTime is NULL and ExpirationTime> NOW() limit 1) 'InGroupSituationId',
               (SELECT GroupSituationId FROM {groupMemberTableName} WHERE g.Id = GroupDataId and CreatorId= '{userId}' and SuccessTime is NULL and ExpirationTime< NOW() ORDER BY CreateTime limit 1) 'FailedGroupSituationId'";
        }
        else
        {
            sql += $"\n'{Guid.Empty}' SuccessGroupSituationId,\n'{Guid.Empty}' InGroupSituationId,\n'{Guid.Empty}' FailedGroupSituationId";
        }
        sql += $"\nFROM {groupDataTableName} g {sqlWhere}";

        return await ExecuteSqlQueryAsync(sql);
    }

    /// <summary>
    /// 我的资料查询
    /// </summary>
    /// <param name="subjectIds"></param>
    /// <param name="takeCount"></param>
    /// <returns></returns>
    public async Task<DataTable> GetMyGroupDataList()
    {
        var groupDataTableName = EngineContext.Current.GetEntityShardingTableParameter<GroupData>().GetCurrentShardingTableName();
        var groupSituationTableName = EngineContext.Current.GetEntityShardingTableParameter<GroupSituation>().GetCurrentShardingTableName();
        var groupMemberTableName = EngineContext.Current.GetEntityShardingTableParameter<GroupMember>().GetCurrentShardingTableName();
        var tenantId = EngineContext.Current.ClaimManager.GetTenantId();
        var userId = EngineContext.Current.ClaimManager.GetUserId();


        var sqlWhere = new StringBuilder();
        sqlWhere.Append($"where g.TenantId='{tenantId}'");

        var sql =
            @$"Select * from (SELECT g.Id,g.DataName,g.PeopleNumber,g.BasePeopleNumber,( SELECT count(1) FROM {groupSituationTableName} WHERE g.Id = GroupDataId && SuccessTime IS NOT NULL) 'SuccessCount',
               (SELECT GroupSituationId FROM {groupMemberTableName} WHERE g.Id = GroupDataId and CreatorId= '{userId}' and SuccessTime is not NULL limit 1) 'SuccessGroupSituationId'
               FROM {groupDataTableName} g {sqlWhere}) g1 Where g1.SuccessGroupSituationId is not null;";

        return await ExecuteSqlQueryAsync(sql);
    }

    private static async Task<DataTable> ExecuteSqlQueryAsync(string sql)
    {
        var dbContext = EngineContext.Current.Resolve<GroupBookingDbContext>();

        var connection = dbContext.Database.GetDbConnection() as MySqlConnection;
        if (connection.State != ConnectionState.Open) await connection.OpenAsync();
        var adapter = new MySqlDataAdapter(sql, connection);
        var dt = new DataTable();
        adapter.Fill(dt);
        connection.Close();
        return dt;
    }

    public override async Task<List<GroupData>> GetByQueryAsync(QueryBase<GroupData> query)
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