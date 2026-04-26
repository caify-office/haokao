using ShortUrlService.Infrastructure.Extensions;
using ShortUrlService.WebApi.Configurations;
using System.Text;

namespace ShortUrlService.WebApi.Shorteners;

/// <summary>
/// https://www.cnblogs.com/xmlnode/p/4544302.html
/// </summary>
public class Base62Shortener(IOptionsSnapshot<ShortUrlConfig> option)
{
    private readonly string _base62CharSet = option.Value.Secrect;
    private readonly int _codeLength = option.Value.CodeLength;

    /// <summary>
    /// 补充0的长度
    /// </summary>
    private int ZeroLength => MaxValue.ToString().Length;

    /// <summary>
    /// Code长度位数下能达到的最大值
    /// </summary>
    private ulong MaxValue
    {
        get
        {
            var max = (ulong)Math.Pow(62, _codeLength) - 1;
            return (ulong)Math.Pow(10, max.ToString().Length - 1) - 1;
        }
    }

    /// <summary>
    /// 【混淆加密】转短码
    /// 1、根据自增主键id前面补0，如：00000123
    /// 2、倒转32100000
    /// 3、把倒转后的十进制转六十二进制（乱序后）
    /// </summary>
    public string Short(long id)
    {
        var idChars = id.ToString()
                        .PadLeft(ZeroLength, '0')
                        .ToCharArray()
                        .Reverse();

        var confuseId = ulong.Parse(string.Join("", idChars));
        var base62Str = Encode(confuseId);
        return base62Str.PadLeft(_codeLength, _base62CharSet.First());
    }

    /// <summary>
    /// 【恢复混淆】解析短码
    /// 1、六十二进制转十进制，得到如：32100000
    /// 2、倒转00000123，得到123
    /// 3、根据123作为主键去数据库查询映射对象
    /// </summary>
    public long Restore(string key)
    {
        var confuseId = Decode(key);
        var idChars = confuseId.ToString()
                               .PadLeft(ZeroLength, '0')
                               .ToCharArray()
                               .Reverse();

        return long.Parse(string.Join("", idChars));
    }

    /// <summary>
    /// 十进制 -> 62进制
    /// </summary>
    public string Encode(ulong value)
    {
        var sb = new StringBuilder();
        do
        {
            sb.Insert(0, _base62CharSet[(int)(value % 62)]);
            value /= 62;
        }
        while (value > 0);

        return sb.ToString();
    }

    /// <summary>
    /// 62进制 -> 十进制
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public long Decode(string value)
    {
        long result = 0;
        for (var i = 0; i < value.Length; i++)
        {
            var power = value.Length - i - 1;
            var digit = _base62CharSet.IndexOf(value[i]);
            if (digit < 0)
            {
                throw new ArgumentException("Invalid character in base62 string", nameof(value));
            }
            result += digit * (long)Math.Pow(62, power);
        }

        return result;
    }

    /// <summary>
    /// 生成随机的0-9a-zA-Z字符串
    /// 62位秘钥
    /// </summary>
    /// <returns></returns>
    public static string GenerateSecret()
    {
        var chars = "0,1,2,3,4,5,6,7,8,9,A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z,a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z".Split(',');
        return string.Join("", chars.ToList().Shuffle());
    }
}