using HaoKao.ProductService.Domain.Enums;

namespace HaoKao.ProductService.Application.ViewModels.Product;

[AutoMapFrom(typeof(Domain.Entities.Product))]
[AutoMapTo(typeof(Domain.Entities.Product))]
public class BrowseProductLiveViewModel : BrowseProductViewModel
{

    /// <summary>
    /// 直播状态
    /// </summary>
    public LiveStatus LiveStatus { get; set; }
    /// <summary>
    /// 直播Id
    /// </summary>
    public Guid LiveId
    {
        get
        {
            return ProductPermissions.FirstOrDefault().PermissionId;
        }

    }

}