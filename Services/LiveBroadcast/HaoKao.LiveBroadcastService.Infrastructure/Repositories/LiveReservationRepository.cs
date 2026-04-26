using HaoKao.LiveBroadcastService.Domain.Entities;
using MySqlConnector;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace HaoKao.LiveBroadcastService.Infrastructure.Repositories;

public class LiveReservationRepository : Repository<LiveReservation>, ILiveReservationRepository
{
    public async Task<Dictionary<Guid, int>> LiveReservationCount(Guid[] productIds)
    {
        var list = await Queryable.Where(x => productIds.Contains(x.ProductId))
                                  .GroupBy(x => x.ProductId)
                                  .Select(g => new
                                  {
                                      g.Key,
                                      Count = g.Count()
                                  }).ToListAsync();
        return list.ToDictionary(x => x.Key, x => x.Count);
    }

    /// <summary>
    /// 查询所有待通知的预约数据
    /// </summary>
    /// <returns></returns>
    public async Task<DataTable> QueryAllReservation()
    {
        var context = EngineContext.Current.Resolve<LiveBroadcastDbContext>();
        var database = context.Database;
        await database.OpenConnectionAsync();
        var connection = database.GetDbConnection();
        var command = connection.CreateCommand();

        command.AddParameter("SchemaName", connection.Database);

        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = "Sp_QueryAllReservation";

        var data = new DataTable();
        var adapter = new MySqlDataAdapter((MySqlCommand)command);
        adapter.Fill(data);
        await database.CloseConnectionAsync();
        return data;
    }

    /// <summary>
    /// 按租户更新预约为已通知状态
    /// </summary>
    /// <param name="tenantId"></param>
    /// <param name="ids"></param>
    public async Task<int> UpdateNotified(Guid tenantId, List<Guid> ids)
    {
        EngineContext.Current.ClaimManager.SetFromDictionary(new Dictionary<string, string>
        {
            { GirvsIdentityClaimTypes.TenantId, tenantId.ToString() },
        });

        var tableName = EngineContext.Current.GetEntityShardingTableParameter<LiveReservation>().GetCurrentShardingTableName();

        var sql = $"UPDATE {tableName} SET Notified = true WHERE Id IN ({string.Join(",", ids.Select(x => $"'{x}'"))})";

        var context = EngineContext.Current.Resolve<LiveBroadcastDbContext>();
        var database = context.Database;
        await database.OpenConnectionAsync();
        var connection = database.GetDbConnection();
        var command = connection.CreateCommand();
        command.CommandText = sql;
        command.CommandType = CommandType.Text;
        var affectedRows = await command.ExecuteNonQueryAsync();
        await database.CloseConnectionAsync();
        return affectedRows;
    }
}