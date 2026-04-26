using Microsoft.International.Converters.PinYinConverter;
using System.Linq;
using System.Text.RegularExpressions;

namespace HaoKao.LiveBroadcastService.Domain.Extensions;

public static class StringExtensions
{
    public static string GetChinesePinyinFirstLetters(this string input)
    {
        var strArray = input.Select(c =>
        {
            // 正则表达式匹配中文字符
            if (Regex.IsMatch(c.ToString(), @"[\u4e00-\u9fa5]"))
            {
                // 如果是中文字符，返回该字符的拼音首字母
                return GetPinyinInitial(c);
            }
            else
            {
                // 如果不是中文字符，返回字符本身
                return c.ToString();
            }
        }).ToArray();
        return string.Join("", strArray);
    }

    // 示例方法，获取单个中文字符的拼音首字母
    // 注意：这里仅为示例，实际应用中应使用拼音库或API
    private static string GetPinyinInitial(char chineseChar)
    {
        // 这里需要一个真实的拼音转换逻辑，这里只是一个示例
        // 实际应用中可以使用第三方库如NLPIR/ICTCLAS等
        var chinese = new ChineseChar(chineseChar);
        var pinyin = chinese.Pinyins[0].ToString();
        return pinyin.Substring(0, 1).ToLower();
    }
}