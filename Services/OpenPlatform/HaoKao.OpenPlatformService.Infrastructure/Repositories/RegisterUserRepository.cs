using Girvs.BusinessBasis.Queries;
using Girvs.Extensions.Collections;
using MySqlConnector;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace HaoKao.OpenPlatformService.Infrastructure.Repositories;

public class RegisterUserRepository(OpenPlatformDbContext dbContext) : Repository<RegisterUser>, IRegisterUserRepository
{
    public override async Task<List<RegisterUser>> GetByQueryAsync(QueryBase<RegisterUser> query)
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

    public Task<bool> ExistByExternalIdentity(string scheme, string uniqueIdentifier)
    {
        return dbContext.ExternalIdentities.AnyAsync(x => x.Scheme == scheme && x.UniqueIdentifier == uniqueIdentifier);
    }

    public Task<ExternalIdentity> GetByExternalIdentity(string scheme, string uniqueIdentifier)
    {
        return dbContext.ExternalIdentities.Where(x => x.Scheme == scheme && x.UniqueIdentifier == uniqueIdentifier)
                                           .Include(x => x.RegisterUser)
                                           .FirstOrDefaultAsync();
    }

    public Task<RegisterUser> GetByInclude(Expression<Func<RegisterUser, bool>> predicate)
    {
        return Queryable.Include(x => x.ExternalIdentities).FirstOrDefaultAsync(predicate);
    }

    public Task<List<RegisterUser>> GetAllWithWeiXin()
    {
        return Queryable.Include(x => x.ExternalIdentities).Where(x => x.ExternalIdentities.Any(y => y.Scheme == "WeiXin")).ToListAsync();
    }

    /// <summary>
    /// 获取总注册用户数, 今日注册用户数, 今日活跃用户数
    /// </summary>
    /// <returns></returns>
    public async Task<(int Total, int Today, int Active)> QueryRegisteredCountAndActiveCount()
    {
        var database = EngineContext.Current.Resolve<OpenPlatformDbContext>().Database;

        var conn = database.GetDbConnection();
        await database.OpenConnectionAsync();

        var sql = @$"
SELECT COUNT(0) Total,
       SUM(CASE WHEN DATE_FORMAT(CreateTime,'%Y-%m-%d') = @Today THEN 1 ELSE 0 END) Today,
       (SELECT COUNT(0) FROM {nameof(DailyActiveUserLog)} WHERE CreateDate = @Today) Active
FROM {nameof(RegisterUser)} LIMIT 1";
        var cmd = conn.CreateCommand();
        cmd.CommandText = sql;
        cmd.AddParameter("Today", DateTime.Now.ToString("yyyy-MM-dd"));

        var reader = await cmd.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return (reader.GetInt32(0), reader.GetInt32(1), reader.GetInt32(2));
        }

        return (0, 0, 0);
    }

    /// <summary>
    /// 每日注册用户走势
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <param name="prev"></param>
    /// <param name="next"></param>
    /// <returns></returns>
    public async Task<Dictionary<string, int>> QueryDailyRegisteredUserTrend(DateTime? start,
                                                                             DateTime? end,
                                                                             DateTime? prev,
                                                                             DateTime? next)
    {
        var database = EngineContext.Current.Resolve<OpenPlatformDbContext>().Database;

        var conn = database.GetDbConnection();
        await database.OpenConnectionAsync();
        var cmd = conn.CreateCommand();

        var whereSql = BuildSqlWhere(cmd, start, end, prev, next);
        var querySql = @$"
SELECT t.`Date`, COUNT(0) Count
FROM (SELECT DATE_FORMAT(u.CreateTime, '%Y-%m-%d') `Date`,
             u.CreateTime,
             u.ClientId
      FROM {TableName} u) t
{whereSql}
GROUP BY t.`Date`
";

        cmd.CommandText = querySql;

        var dict = GetDates(start, end, prev, next).ToDictionary(x => x.ToString("yyyy-MM-dd"), x => 0);
        var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            var date = reader.GetString(0);
            if (dict.ContainsKey(date))
            {
                dict[date] = Convert.ToInt32(reader["Count"]);
            }
        }

        return dict;

        static string BuildSqlWhere(DbCommand cmd,
                                    DateTime? start,
                                    DateTime? end,
                                    DateTime? prev,
                                    DateTime? next)
        {
            var conditions = new List<string>(2);
            if (start.HasValue)
            {
                conditions.Add("t.CreateTime >= @Start\n");
                cmd.AddParameter("Start", start.Value.ToString("yyyy-MM-dd"));
            }
            if (end.HasValue)
            {
                conditions.Add("t.CreateTime <= @End\n");
                cmd.AddParameter("End", end.Value.AddDays(1).ToString("yyyy-MM-dd"));
            }
            if (prev.HasValue)
            {
                var dates = GetWeekDates(prev.Value.AddDays(-1), false);
                conditions.Add($"t.`Date` IN ({string.Join(", ", dates.Select(d => $"'{d:yyyy-MM-dd}'"))})\n");
            }
            if (next.HasValue)
            {
                var dates = GetWeekDates(next.Value.AddDays(1), true);
                conditions.Add($"t.`Date` IN ({string.Join(", ", dates.Select(d => $"'{d:yyyy-MM-dd}'"))})\n");
            }
            if (conditions.Any())
            {
                return "WHERE " + string.Join("  AND ", conditions).TrimEnd('\n');
            }
            return string.Empty;
        }

        static DateTime[] GetDates(DateTime? start,
                                   DateTime? end,
                                   DateTime? prev,
                                   DateTime? next)
        {
            if (prev.HasValue)
            {
                return GetWeekDates(prev.Value.AddDays(-1), false);
            }
            if (next.HasValue)
            {
                return GetWeekDates(next.Value.AddDays(1), true);
            }
            if (start.HasValue && end.HasValue)
            {
                return GetSerialDates(start.Value, end.Value);
            }
            return GetWeekDates(DateTime.Now, false);
        }

        static DateTime[] GetSerialDates(DateTime start, DateTime end)
        {
            var dates = new List<DateTime>();
            for (var date = end; date >= start; date = date.AddDays(-1))
            {
                dates.Add(date);
            }
            return dates.OrderBy(x => x).ToArray();
        }

        static DateTime[] GetWeekDates(DateTime inputDate, bool isNextWeek)
        {
            var dates = new DateTime[7];
            for (var i = 0; i < 7; i++)
            {
                if (isNextWeek)
                {
                    dates[i] = inputDate.AddDays(i);
                }
                else
                {
                    dates[i] = inputDate.AddDays(-i);
                }
            }
            return dates.OrderBy(x => x).ToArray();
        }
    }

    /// <summary>
    /// 查询用户注册客户端分组数据
    /// </summary>
    /// <returns></returns>
    public async Task<DataTable> QueryRegisteredUserClientGrouping(DateTime? start, DateTime? end)
    {
        var database = EngineContext.Current.Resolve<OpenPlatformDbContext>().Database;

        var conn = database.GetDbConnection();
        await database.OpenConnectionAsync();
        var cmd = conn.CreateCommand();

        var whereSql = BuildSqlWhere(cmd, start, end);
        var querySql = @$"
SELECT ClientId, COUNT(0) Count
FROM (SELECT ClientId FROM {TableName} {whereSql}) t
GROUP BY ClientId
";

        cmd.CommandText = querySql;
        var data = new DataTable();
        var adapter = new MySqlDataAdapter((MySqlCommand)cmd);
        adapter.Fill(data);
        return data;

        static string BuildSqlWhere(DbCommand cmd, DateTime? start, DateTime? end)
        {
            var conditions = new List<string>(2);
            if (start.HasValue)
            {
                conditions.Add("CreateTime >= @Start");
                cmd.AddParameter("Start", start.Value.ToString("yyyy-MM-dd"));
            }
            if (end.HasValue)
            {
                conditions.Add("CreateTime <= @End");
                cmd.AddParameter("End", end.Value.AddDays(1).ToString("yyyy-MM-dd"));
            }
            if (conditions.Any())
            {
                return "WHERE " + string.Join(" AND ", conditions);
            }
            return string.Empty;
        }
    }

    private static string TableName => EngineContext.Current.GetEntityShardingTableParameter<RegisterUser>().GetCurrentShardingTableName();
}

public static class DbCommandExtension
{
    public static void AddParameter(this DbCommand cmd, string name, object value)
    {
        var parameter = cmd.CreateParameter();
        parameter.ParameterName = name;
        parameter.Value = value;
        cmd.Parameters.Add(parameter);
    }
}