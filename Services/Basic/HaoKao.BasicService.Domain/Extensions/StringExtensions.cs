namespace HaoKao.BasicService.Domain.Extensions;

public static class StringExtensions
{
    public static string DecodeBase64(this string source)
    {
        try
        {
            if (source.IsNullOrWhiteSpace())
            {
                return source;
            }
            var bytes = Convert.FromBase64String(source);
            return Encoding.UTF8.GetString(bytes);
        }
        catch
        {
            return source;
        }
    }
}