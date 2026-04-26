using HaoKao.ProductService.Domain.Enums;

namespace HaoKao.ProductService.Application.ViewModels.Product;

public class QueryProductByLiveIdViewModel
{
    /// <summary>
    /// 直播Id
    /// </summary>
    public Guid LiveId { get; set; }

    /// <summary>
    /// 直播状态
    /// </summary>
    public LiveStatus LiveStatus { get; set; }

}