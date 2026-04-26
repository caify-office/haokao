using System.Text;

namespace ShortUrlService.WebApi.Shorteners;

public static class HashShortener
{
    private static readonly char[] _chars =
    [
        '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
        'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z',
        'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z',
    ];

    private static readonly int _size = _chars.Length;

    private static string ConvertDecToBase62(long num)
    {
        StringBuilder sb = new();
        while (num > 0)
        {
            var i = (int)(num % _size);
            sb.Append(_chars[i]);
            num /= _size;
        }
        return ReverseString(sb.ToString());
    }

    private static string ReverseString(string str)
    {
        var array = str.ToCharArray();
        Array.Reverse(array);
        return new string(array);
    }

    public static string Short(string str)
    {
        var i = MurmurHash.Hash32(str);
        var num = i < 0 ? int.MaxValue - (long)i : i;
        return ConvertDecToBase62(num);
    }
}

// MurmurHash implementation (assuming MurmurHash.Hash32 equivalent in C#)
file static class MurmurHash
{
    public static int Hash32(string text)
    {
        const uint seed = 0x9747b28c;
        var data = Encoding.UTF8.GetBytes(text);
        var len = data.Length;
        const int m = 0x5bd1e995;
        const int r = 24;
        var h = seed ^ (uint)len;
        var len_4 = len / 4;

        for (var i = 0; i < len_4; i++)
        {
            var i_4 = i * 4;
            var k = BitConverter.ToUInt32(data, i_4);
            k *= m;
            k ^= k >> r;
            k *= m;
            h *= m;
            h ^= k;
        }

        var len_m = len_4 * 4;
        var left = len - len_m;
        if (left != 0)
        {
            if (left >= 3)
            {
                h ^= (uint)data[len - 3] << 16;
            }
            if (left >= 2)
            {
                h ^= (uint)data[len - 2] << 8;
            }
            if (left >= 1)
            {
                h ^= data[len - 1];
            }
            h *= m;
        }

        h ^= h >> 13;
        h *= m;
        h ^= h >> 15;

        return (int)h;
    }
}