using System.Globalization;
using System.Text.RegularExpressions;

namespace HaoKao.Common.Extensions;

public static class StringExtensions
{
    public static void SetTenantId(this string table)
    {
        var tenantId = Guid.Parse(table.Split('_')[1].Insert(8, "-").Insert(13, "-").Insert(18, "-").Insert(23, "-")).ToString();
        EngineContext.Current.ClaimManager.SetFromDictionary(new() { { GirvsIdentityClaimTypes.TenantId, tenantId }, });
    }
    /// <summary>
    /// 过滤URL不支持的特殊字符
    /// </summary>
    /// <param name="input">原始字符串</param>
    /// <param name="replacement">替换字符（默认为空字符串）</param>
    /// <returns>符合URL规范的字符串</returns>
    public static string SanitizeForUrl(this string input, string replacement = "")
    {
        if (string.IsNullOrEmpty(input)) return input;

        // 正则表达式模式说明：
        // - 匹配所有非字母数字、非连接符、非点、非下划线、非波浪号的字符
        // - 使用编译模式优化性能
        const string pattern = @"[^\w\-\.~]";

        // 替换连续非法字符为单个替换字符
        var sanitized = Regex.Replace(input, pattern + "+", replacement);

        // 移除首尾替换字符（当替换符非空时）
        if (!string.IsNullOrEmpty(replacement))
        {
            sanitized = sanitized.Trim(replacement[0]);
        }

        return sanitized;
    }
    /// <summary>
    /// 将零时区时间字符串转为本地时间字符串
    /// </summary>
    /// <param name="utcTimeString"></param>
    /// <returns></returns>
    public static string ConvertToLocalTime(this string utcTimeString)
    {
        // 解析为DateTime（指定为UTC类型）
        DateTime utcTime = DateTime.Parse(utcTimeString, null, DateTimeStyles.AdjustToUniversal);

        // 转换为本地时间
        DateTime localTime = utcTime.ToLocalTime();

        // 格式化为字符串
        string localTimeString = localTime.ToString("yyyy-MM-dd HH:mm:ss");
        return localTimeString;
    }
}