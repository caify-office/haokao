using HaoKao.LiveBroadcastService.Domain.Entities;
using System.Data;
using System.Threading.Tasks;

namespace HaoKao.LiveBroadcastService.Infrastructure.Repositories;

public class LiveMessageRepository : Repository<LiveMessage>, ILiveMessageRepository
{
    /// <summary>
    /// 新增消息
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    public Task AddMessage(LiveMessage message)
    {
        var tableName = EngineContext.Current.GetEntityShardingTableParameter<LiveMessage>().GetCurrentShardingTableName();
        var sql = $"""
                   INSERT INTO `{tableName}` (`Id`, `Content`, `CreateTime`, `CreatorId`, `CreatorName`, `LiveId`, `LiveMessageType`, `TenantId`)
                   VALUES ('{message.Id}', '{message.Content}', '{DateTime.Now:yyyy-MM-dd HH:mm:ss}', '{message.CreatorId}', '{message.CreatorName}', '{message.LiveId}', '{(int)message.LiveMessageType}', '{message.TenantId}');
                   """;
        return ExecuteNonQueryAsync(sql);
    }

    private static async Task<int> ExecuteNonQueryAsync(string sql)
    {
        var context = EngineContext.Current.Resolve<LiveBroadcastDbContext>();
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