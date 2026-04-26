using Microsoft.Extensions.DependencyInjection;

namespace HaoKao.UserAnswerRecordService.Infrastructure;

internal static class DbCommandExtension
{
    public static void AddParameter(this IDbCommand cmd, string name, object value)
    {
        var parameter = cmd.CreateParameter();
        parameter.Direction = ParameterDirection.Input;
        parameter.ParameterName = name;
        parameter.Value = value;
        cmd.Parameters.Add(parameter);
    }
}

internal static class Extensions
{
    public static async Task<IReadOnlyList<string>> GetTableNameList(this DbContext dbContext, string schema)
    {
        var tableName = $"{schema}_".ToLower();
        var conn = dbContext.Database.GetDbConnection();
#pragma warning disable CA1862 // 使用 "StringComparison" 方法重载来执行不区分大小写的字符串比较
        var list = await dbContext.Database.SqlQuery<InformationSchema>($"SELECT `TABLE_NAME`, `TABLE_SCHEMA` FROM information_schema.`TABLES`")
                                  .Where(x => x.TABLE_SCHEMA.Equals(conn.Database)
                                           && x.TABLE_NAME.ToLower().StartsWith(tableName))
                                  .ToListAsync();
#pragma warning restore CA1862 // 使用 "StringComparison" 方法重载来执行不区分大小写的字符串比较
        return list.Select(x => x.TABLE_NAME).ToList();
    }

    public static AsyncServiceScope CreateTenantAsyncScope(this IServiceProvider provider, string table)
    {
        var tenantId = Guid.Parse(table.Split('_')[1].Insert(8, "-").Insert(13, "-").Insert(18, "-").Insert(23, "-")).ToString();
        EngineContext.Current.ClaimManager.SetFromDictionary(new() { { GirvsIdentityClaimTypes.TenantId, tenantId } });
        return provider.CreateAsyncScope();
    }
}

internal class InformationSchema
{
    public string TABLE_NAME { get; set; }

    public string TABLE_SCHEMA { get; set; }
}