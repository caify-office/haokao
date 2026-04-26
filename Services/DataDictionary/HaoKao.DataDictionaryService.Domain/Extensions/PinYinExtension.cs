using System.Text.RegularExpressions;

namespace HaoKao.DataDictionaryService.Domain.Extensions;

public static class PinYinExtension
{
    public static string PinYinCode(this string str)
    {
        // 使用正则表达式匹配标点符号和特殊字符
        str = Regex.Replace(str, @"[\s\p{P}]", "");
        var py = str.PinYinFirst();
        var code = py.Length > 2 ? $"{py[..2]}{CommonHelper.GenerateRandomDigitCode(4)}" : $"{py}{CommonHelper.GenerateRandomDigitCode(5)}";
        return code;
    }

    /// <summary>
    /// 汉字转化为拼音首字母
    /// </summary>
    /// <param name="str">汉字</param>
    /// <returns>首字母</returns>
    public static string PinYinFirst(this string str)
    {
        var r = string.Empty;
        foreach (var obj in str)
        {
            if (obj.ToString().IsEn())
            {
                r += obj.ToString().ToUpper();
            }
            else if (obj.ToString().IsInt())
            {
                r += obj.ToString();
            }
            else
            {
                var chineseChar = new ChineseChar(obj);
                var t = chineseChar.Pinyins[0];
                r += t[..1];
            }
        }
        return r;
    }

    /// <summary>
    /// 判断字符串是否是英文
    /// </summary>
    /// <param name="content"></param>
    /// <returns></returns>
    public static bool IsEn(this string content)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            return false;
        }
        var bytes = Encoding.UTF8.GetBytes(content);
        var result = bytes.Length == content.Length;
        return result;
    }

    /// <summary>
    /// 判断字符串是否是数字
    /// </summary>
    /// <param name="content"></param>
    /// <returns></returns>
    public static bool IsInt(this string content)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            return false;
        }
        return int.TryParse(content, out var result);
    }
}