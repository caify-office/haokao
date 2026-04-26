using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Options;
using System.Text.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace HaoKao.Common.Extensions;

public static class DbContextExtensions
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
}

internal class InformationSchema
{
    public string TABLE_NAME { get; set; }

    public string TABLE_SCHEMA { get; set; }
}

public static class JsonConversion
{
    public static ValueConverter<T, string> Create<T>()
    {
        var jsonSerializerOptions = EngineContext.Current.Resolve<IOptions<JsonSerializerOptions>>();
        return new ValueConverter<T, string>(
            v => JsonSerializer.Serialize(v, jsonSerializerOptions.Value),
            v => !string.IsNullOrEmpty(v)
                ? JsonSerializer.Deserialize<T>(v, jsonSerializerOptions.Value)
                : default
        );
    }
}