using Attribute = System.Attribute;

namespace HaoKao.OrderService.Application.PayHandler.Config;

public class ConfigTipAttribute(string tip) : Attribute
{
    public string Tip { get; } = tip;
}