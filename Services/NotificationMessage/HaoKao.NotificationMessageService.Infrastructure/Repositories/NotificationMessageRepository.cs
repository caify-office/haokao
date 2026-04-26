using HaoKao.NotificationMessageService.Infrastructure.Refit;

namespace HaoKao.NotificationMessageService.Infrastructure.Repositories;

public class NotificationMessageRepository : Repository<NotificationMessage>, INotificationMessageRepository
{
    public async Task GetByQueryResultSpAsync(NotificationMessageQuery query)
    {
        var connectionString = EngineContext.Current.GetAppModuleConfig<DbConfig>()
            .GetDataConnectionConfig(typeof(NotificationMessageDbContext))
            .GetSecureRandomReadDataConnectionString();
        var schemaName = GetSchemaName(connectionString);

        var conn = new MySqlConnection(connectionString);

        var cmd = new MySqlCommand("Sp_QueryAllSiteMessage", conn);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.Add(new MySqlParameter("IdCard", query.IdCard));

        //此处需要同步更新存储过程，待吴勇飞进行更新
        cmd.Parameters.Add(new MySqlParameter("TenantAccessId", query.TenantAccessId ?? Guid.Empty));
        cmd.Parameters.Add(new MySqlParameter("SchemaName", schemaName));
        cmd.Parameters.Add(new MySqlParameter("startt", (query.PageIndex - 1) * query.PageSize));
        cmd.Parameters.Add(new MySqlParameter("endd", query.PageSize));
        var pOut = new MySqlParameter("count", SqlDbType.Int);
        pOut.Direction = ParameterDirection.Output;
        cmd.Parameters.Add(pOut);
        await conn.OpenAsync();
        MySqlDataReader rs = cmd.ExecuteReader(CommandBehavior.CloseConnection);
        var resuls = new List<NotificationMessage>();
        if (rs.HasRows)
        {
            while (rs.Read())
            {
                resuls.Add(new NotificationMessage
                {
                    Id = rs["Id"].ToString().ToGuid(),
                    Title = rs["Title"].ToString(),
                    ParameterContent = rs["ParameterContent"].ToString(),
                    CreateTime = DateTime.Parse(rs["CreateTime"].ToString()),
                    IsRead = rs["IsRead"].ToString() == "1" ? true : false,
                    TenantId = rs["TenantId"].ToString().ToGuid(),
                });
            }
        }

        query.Result = resuls;
        await cmd.Connection.CloseAsync();
        query.RecordCount = int.Parse(pOut.Value.ToString());
    }

    public async Task<int> GetUnReadCountSpAsync(string idCard, Guid tenantAccessId)
    {
        var connectionString = EngineContext.Current.GetAppModuleConfig<DbConfig>()
            .GetDataConnectionConfig(typeof(NotificationMessageDbContext))
            .GetSecureRandomReadDataConnectionString();
        var schemaName = GetSchemaName(connectionString);

        var conn = new MySqlConnection(connectionString);
        var cmd = new MySqlCommand("Sp_QueryAllSiteUnReadCount", conn);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.Add(new MySqlParameter("IdCard", idCard));
        //此处需要同步更新存储过程，待吴勇飞进行更新
        cmd.Parameters.Add(new MySqlParameter("TenantAccessId", tenantAccessId));
        cmd.Parameters.Add(new MySqlParameter("SchemaName", schemaName));
        var pOut = new MySqlParameter("count", SqlDbType.Int);
        pOut.Direction = ParameterDirection.Output;
        cmd.Parameters.Add(pOut);
        await conn.OpenAsync();
        MySqlDataReader rs = cmd.ExecuteReader(CommandBehavior.CloseConnection);
        await cmd.Connection.CloseAsync();
        ;
        return int.Parse(pOut.Value.ToString());
    }

    private string GetSchemaName(string connectionString)
    {
        string beginStr1 = "database=";
        string beginStr2 = "DataBase=";
        string endStr = ";";
        var result = string.Empty;
        try
        {
            result = System.Text.RegularExpressions.Regex.Match(connectionString, $"{beginStr1}(.*?){endStr}")
                .Result("$1");
        }
        catch
        {
            result = System.Text.RegularExpressions.Regex.Match(connectionString, $"{beginStr2}(.*?){endStr}")
                .Result("$1");
        }

        //var result = "Wb_ExamineeManagement_2022";

        return result;
    }


    public async Task<bool> FollowWeChat(WechatMessageSetting setting, string openId)
    {
        try
        {
            var weChatRemoteService = EngineContext.Current.RestService<IWeChatRemoteService>();
            var tokenResponse = await GetWeChatTokenResponse(setting);

            if (string.IsNullOrWhiteSpace(tokenResponse.Access_token))
                return false;

            var sendMessageResultStr =
                await weChatRemoteService.GetUserInfo(tokenResponse.Access_token, openId);

            var sendMessageResultResponse = JsonConvert.DeserializeObject<WechatUserInfo>(sendMessageResultStr);

            return sendMessageResultResponse?.Subscribe == 1;
        }
        catch (Exception ex)
        {
            var logger = EngineContext.Current.Resolve<ILogger<NotificationMessageRepository>>();
            logger.LogError(ex.Message, ex);
            return false;
        }
    }

    public async Task<string> GetOpenIdAsync(WechatMessageSetting wechatMessageSetting, string code)
    {
        var weChatRemoteService = EngineContext.Current.RestService<IWeChatRemoteService>();
        var result =
            await weChatRemoteService.GetOpenIdAsync(wechatMessageSetting.AppId, wechatMessageSetting.AppSecret,
                code);
        var response = JsonConvert.DeserializeObject<WeChatResponse>(result);
        return response?.OpenId ?? "";
    }

    public class WechatUserInfo
    {
        public int Subscribe { get; set; }
    }

    private CacheKey BuilderCacheKey
    {
        get
        {
            var cacheKey = GirvsEntityCacheDefaults<WechatMessageSetting>.ByIdCacheKey.Create("WeChatToken");
            cacheKey.CacheTime = 120;
            return cacheKey;
        }
    }

    private async Task<WeChatResponse> GetWeChatTokenResponse(WechatMessageSetting setting)
    {
        var weChatTokenResponse = await GetWechatRemoteTokenResponse(setting);
        return weChatTokenResponse;
        // var weChatRemoteService = EngineContext.Current.RestService<IWeChatRemoteService>();
        // var cacheManager = EngineContext.Current.Resolve<IStaticCacheManager>();
        // var weChatTokenResponse = await cacheManager.GetAsync(BuilderCacheKey,
        //     async () => await GetWechatRemoteTokenResponse(setting));



        // var expireTime =
        //     weChatTokenResponse.DateTimeNow.AddSeconds(Convert.ToInt32(weChatTokenResponse.Expires_in) - (30 * 60));
        //
        // if (DateTime.Now <= expireTime) return weChatTokenResponse;
        //
        // weChatTokenResponse = await GetWechatRemoteTokenResponse(setting);
        // cacheManager.Set(BuilderCacheKey, weChatTokenResponse);

        // return weChatTokenResponse;
    }

    private async Task<WeChatResponse> GetWechatRemoteTokenResponse(WechatMessageSetting setting)
    {
        var weChatRemoteService = EngineContext.Current.RestService<IWeChatRemoteService>();
        var tokenResponseString = await weChatRemoteService.GetAccessTokenAsync(setting.AppId, setting.AppSecret);
        return JsonConvert.DeserializeObject<WeChatResponse>(tokenResponseString);
    }
}