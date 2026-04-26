using System.Text.Json.Serialization;

namespace HaoKao.ProductService.Application.ViewModels.Product;

[AutoMapFrom(typeof(Domain.Entities.Product))]
[AutoMapTo(typeof(Domain.Entities.Product))]
public class BrowseProductPriceViewModel : IDto
{
    public Guid Id { get; set; }
    /// <summary>
    /// 是否过期
    /// </summary>
    [JsonIgnore]
    public bool IsExpire { get; set; }
    /// <summary>
    /// 价格
    /// </summary>
    public double Price { get; set; }

    /// <summary>
    /// 优惠价格
    /// </summary>
    [JsonIgnore]
    public double DiscountedPrice { get; set; }
    /// <summary>
    /// 显示价格
    /// </summary>
    public double DisplayPrice 
    { 
        get 
        {
            return IsExpire ? Price : DiscountedPrice; 
        } 
    }




}