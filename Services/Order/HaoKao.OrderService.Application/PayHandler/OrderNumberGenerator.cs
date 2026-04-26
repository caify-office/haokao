using System.Text;

namespace HaoKao.OrderService.Application.PayHandler;

public static class OrderNumberGenerator
{
    public static string Generate(string prefix)
    {
        // 获取当前日期时间
        var now = DateTime.Now;

        // 生成日期时间部分
        var dateTimePart = now.ToString("yyyyMMddHHmmssfff");

        // 生成随机数部分
        var randomPart = GenerateRandomNumber(4); // 生成指定位数的随机数

        // 拼接所有部分生成最终的订单号
        var orderNumber = $"{prefix}{dateTimePart}{randomPart}";

        return orderNumber;
    }

    // 生成指定位数的随机数
    private static string GenerateRandomNumber(int length)
    {
        var random = new Random();
        var sb = new StringBuilder();

        for (int i = 0; i < length; i++)
        {
            sb.Append(random.Next(0, 10));
        }

        return sb.ToString();
    }
}