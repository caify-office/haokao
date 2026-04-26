using HaoKao.LiveBroadcastService.Domain.Entities;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace HaoKao.LiveBroadcastService.Infrastructure.Repositories;

public class LiveOnlineUserTrendRepository : Repository<LiveOnlineUserTrend>, ILiveOnlineUserTrendRepository
{
    /// <summary>
    /// 记录在线用户走势数据
    /// </summary>
    /// <param name="interval">记录间隔(分钟)</param>
    /// <returns></returns>
    public async Task TrackOnlineUserTrend(int interval)
    {
        var context = EngineContext.Current.Resolve<LiveBroadcastDbContext>();
        var database = context.Database;
        await database.OpenConnectionAsync();
        var connection = database.GetDbConnection();
        var command = connection.CreateCommand();

        command.AddParameter("SchemaName", connection.Database);
        command.AddParameter("Interval", interval);

        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = "Sp_TrackOnlineUserTrend";

        await command.ExecuteNonQueryAsync();
        await database.CloseConnectionAsync();
    }
}

public static class DbCommandExtension
{
    public static void AddParameter(this DbCommand cmd, string name, object value)
    {
        var parameter = cmd.CreateParameter();
        parameter.Direction = ParameterDirection.Input;
        parameter.ParameterName = name;
        parameter.Value = value;
        cmd.Parameters.Add(parameter);
    }
}