using HaoKao.ProductService.Domain.Entities;

namespace HaoKao.ProductService.Domain.Repositories;

public interface IProductPackageRepository : IRepository<ProductPackage>
{
    Task<List<Guid>> GetAllProductIds();
}

public class ProductPackageCompositeAttribute
{
    /// <summary>
    /// 产品包Id
    /// </summary>
    public Guid ProductPackageId { get; set; }

    /// <summary>
    /// 价格
    /// </summary>
    public double Price { get; set; }

    /// <summary>
    /// 产品卡片图片
    /// </summary>
    public string CardImage { get; set; }

    /// <summary>
    /// 详细介绍图片
    /// </summary>
    public string DetailImage { get; set; }

    /// <summary>
    /// 购买人数配置
    /// </summary>
    public int NumberOfBuyers { get; set; }

    /// <summary>
    /// 最低优惠价格
    /// </summary>
    [JsonIgnore]
    [Newtonsoft.Json.JsonIgnore]
    public double DiscountedPrice { get; set; }

    public double DisplayPrice => DateTime.Now <= PreferentialExpiryTime ? DiscountedPrice : Price;

    /// <summary>
    /// 讲师列表
    /// </summary>
    public List<Guid> LecturerList { get; set; }



    /// <summary>
    /// 优惠到期时间
    /// </summary>
    [JsonIgnore]
    [Newtonsoft.Json.JsonIgnore]
    public DateTime PreferentialExpiryTime { get; set; }
}