namespace HaoKao.NotificationMessageService.Infrastructure.Repositories;

public class RegisteredUserRepository : IRegisteredUserRepository
{
    public async Task<RegisteredUser> GetRegisteredUser(string cardId, string contactNumber)
    {
        var dbContext = EngineContext.Current.Resolve<NotificationMessageDbContext>();
        var conn = dbContext.Database.GetDbConnection() as MySqlConnection;
        var sql =
            "select UserName,CardId,ContactNumber,OpenId from RegisteredUser where ContactNumber=@contactNumber or CardId=@CardId limit 1";
        var cmd = new MySqlCommand(sql, conn);
        await conn.OpenAsync();
        cmd.Parameters.Add("@contactNumber", MySqlDbType.VarChar).Value = contactNumber;
        cmd.Parameters.Add("@CardId", MySqlDbType.VarChar).Value = cardId;
        var reader = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection);
        RegisteredUser user = null;
        if (reader.HasRows)
        {
            await reader.ReadAsync();
            user = new RegisteredUser
            {
                UserName = reader["UserName"].ToString(),
                CardId = reader["CardId"].ToString(),
                ContactNumber = reader["ContactNumber"].ToString(),
                OpenId = reader["OpenId"].ToString()
            };
        }

        await conn.CloseAsync();
        return user;
    }
}