using Attribute = System.Attribute;

namespace HaoKao.OrderService.Application.PayHandler.Config;

public class ConfigTypeAttribute(ConfigType configType) : Attribute
{
    public ConfigType ConfigType { get; } = configType;
}

public enum ConfigType
{
    /// <summary>
    /// 字符串
    /// </summary>
    String,

    /// <summary>
    /// 整型
    /// </summary>
    Int,

    /// <summary>
    /// 文件路径
    /// </summary>
    FilePath
}