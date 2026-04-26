using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace HaoKao.CouponService.Infrastructure;

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
    public static async Task<List<string>> GetTableNameList(this DbContext dbContext, string schema)
    {
        var tableName = $"{schema}_".ToLower();
        var conn = dbContext.Database.GetDbConnection();
        var list = await dbContext.Database.SqlQuery<InformationSchema>($"SELECT `TABLE_NAME`, `TABLE_SCHEMA` FROM information_schema.`TABLES`")
                                  .Where(x => x.TABLE_SCHEMA.ToLower() == conn.Database.ToLower()
                                           && x.TABLE_NAME.ToLower().StartsWith(tableName))
                                  .ToListAsync();
        return list.Select(x => x.TABLE_NAME).ToList();
    }
}

internal class InformationSchema
{
    public string TABLE_NAME { get; set; }

    public string TABLE_SCHEMA { get; set; }
}