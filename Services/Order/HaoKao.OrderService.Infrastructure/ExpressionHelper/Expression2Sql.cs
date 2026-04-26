using System.Linq.Expressions;

namespace HaoKao.OrderService.Infrastructure.ExpressionHelper;

/// <summary>
/// lambda表达式转为where条件sql
/// </summary>
public class Expression2Sql
{
    #region Expression 转成 where

    /// <summary>
    /// Expression 转成 Where String
    /// 支持表达式：==、!= 、Equals、Contains、StartsWith、EndsWith、&&、||
    /// 不支持：!（取反）
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="predicate"></param>
    /// <param name="databaseType">数据类型（用于字段是否加引号）</param>
    /// <returns></returns>
    public static string GetWhereByLambda<T>(Expression<Func<T, bool>> predicate, string databaseType = "sqlserver")
    {
        bool withQuotationMarks = GetWithQuotationMarks(databaseType);

        ConditionBuilder conditionBuilder = new ConditionBuilder();
        //字段是否加引号（PostgreSql,Oracle）
        conditionBuilder.SetIfWithQuotationMarks(withQuotationMarks);
        conditionBuilder.SetDataBaseType(databaseType);
        conditionBuilder.Build(predicate);

        for (int i = 0; i < conditionBuilder.Arguments.Length; i++)
        {
            object ce = conditionBuilder.Arguments[i];
            if (ce == null)
            {
                //conditionBuilder.Arguments[i] = DBNull.Value;
                conditionBuilder.Arguments[i] = "''";
            }
            else if (ce is string || ce is char || ce is Guid)
            {
                if (ce.ToString().ToLower().Trim().IndexOf("in(") == 0 ||
                    ce.ToString().ToLower().Trim().IndexOf("not in(") == 0 ||
                    ce.ToString().ToLower().Trim().IndexOf(" like '") == 0 ||
                    ce.ToString().ToLower().Trim().IndexOf("not like") == 0)
                {
                    conditionBuilder.Arguments[i] = $" {ce} ";
                }
                else
                {
                    conditionBuilder.Arguments[i] = $"'{ce}'";
                }
            }
            else if (ce is DateTime)
            {
                conditionBuilder.Arguments[i] = $"'{ce}'";
            }
            else if (ce is int || ce is long || ce is short || ce is decimal || ce is double || ce is float ||
                     ce is bool || ce is byte || ce is sbyte)
            {
                conditionBuilder.Arguments[i] = ce.ToString();
            }
            else if (ce is ValueType)
            {
                conditionBuilder.Arguments[i] = ce.ToString();
            }
            else
            {
                conditionBuilder.Arguments[i] = $"'{ce}'";
            }
        }

        return string.Format(conditionBuilder.Condition, conditionBuilder.Arguments);
    }

    /// <summary>
    /// 获取是否字段加双引号
    /// </summary>
    /// <param name="databaseType"></param>
    /// <returns></returns>
    public static bool GetWithQuotationMarks(string databaseType)
    {
        bool result = false;
        switch (databaseType.ToLower())
        {
            case "postgresql":
            case "oracle":
                result = true;
                break;
        }

        return result;
    }

    #endregion
}