using MySqlConnector;
using System.Data;
using System.Data.Common;

namespace HaoKao.Common.Extensions;

public static class DbCommandExtensions
{
    public static void AddParameter(this DbCommand command, string name, object value)
    {
        var parameter = command.CreateParameter();
        parameter.Direction = ParameterDirection.Input;
        parameter.ParameterName = name;
        parameter.Value = value;
        command.Parameters.Add(parameter);
    }

    public static List<Dictionary<string, object>> GetResult(this DbCommand command)
    {
        var result = new List<Dictionary<string, object>>();
        using var reader = command.ExecuteReader();
        //reader.NextResult();
        // 读取当前页的数据
        while (reader.Read())
        {
            var row = new Dictionary<string, object>();

            for (var i = 0; i < reader.FieldCount; i++)
            {
                row.Add(reader.GetName(i), reader.GetValue(i));
            }

            result.Add(row);
        }

        return result;
    }

    public static MySqlParameter AddOutParameter(this DbCommand command, string parameterName)
    {
        var parameter = new MySqlParameter(parameterName, MySqlDbType.Int32)
        {
            Direction = ParameterDirection.Output,
        };
        command.Parameters.Add(parameter);
        return parameter;
    }
}