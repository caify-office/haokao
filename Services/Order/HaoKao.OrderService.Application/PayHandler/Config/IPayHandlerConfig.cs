namespace HaoKao.OrderService.Application.PayHandler.Config;

public interface IPayHandlerConfig
{
    /// <summary>
    /// 支付方式的图标
    /// </summary>
    string IconUrl { get; }

    /// <summary>
    /// 订单号前缀 长度（2到10位 的字母和数字组成）
    /// </summary>
    string OrderPrefix { get; }

    /// <summary>
    /// 读取配置信息
    /// </summary>
    /// <param name="dictionary"></param>
    void ReadFromDictionary(Dictionary<string, string> dictionary);
}

public class ConfigHandler
{
    public virtual void ReadFromDictionary(Dictionary<string, string> dictionary)
    {
        var properties = GetType().GetProperties();
        foreach (var prop in properties)
        {
            var key = dictionary.Keys.FirstOrDefault(x => string.Equals(x, prop.Name, StringComparison.OrdinalIgnoreCase));
            if (string.IsNullOrEmpty(key)) continue;
            var value = dictionary[key];
            prop.SetValue(this, value);
        }
    }
}