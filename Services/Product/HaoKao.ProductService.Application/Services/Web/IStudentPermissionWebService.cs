using HaoKao.ProductService.Application.ViewModels.StudentPermission;
using HaoKao.ProductService.Domain.Enums;

namespace HaoKao.ProductService.Application.Services.Web;

public interface IStudentPermissionWebService : IAppWebApiService, IManager
{
    /// <summary>
    ///  获取当前用户买过的产品id,名称和对应的协议id
    /// </summary>
    /// <returns></returns>
    Task<List<AgreementListViewModel>> GetAllAgreement();

    /// <summary>
    /// 根据产品Id获取用户权限
    /// </summary>
    /// <param name="productId">产品Id</param>
    Task<BrowseStudentPermissionViewModel> GetByProductId(Guid productId);

    /// <summary>
    /// 验证当前用户在当前科目下是否具有有产品的权限
    /// </summary>
    /// <param name="productType">产品类型</param>
    /// <param name="subjectId">科目id</param>
    /// <param name="premissionId">权限id</param>
    /// <returns></returns>
    Task<bool> HasAuthority(ProductType productType, Guid? subjectId, Guid? premissionId);

    /// <summary>
    /// 获取关联当前权限Id的产品Id
    /// </summary>
    /// <param name="subjectId">科目id</param>
    /// <param name="premissionId">权限id</param>
    /// <returns></returns>
    Task<List<Guid>> GetProductIds(Guid? subjectId, Guid? premissionId);

    /// <summary>
    /// 判断是否有当前产品的权限
    /// </summary>
    /// <param name="productType">产品类别</param>
    /// <param name="productId">产品Id</param>
    Task<bool> HasProductAuthority(ProductType productType, Guid productId);

    /// <summary>
    /// 判断是否有当前直播的权限
    /// </summary>
    /// <param name="liveId">直播Id</param>
    Task<bool> HasLiveAuthority(Guid liveId);

    /// <summary>
    /// 创建学员权限
    /// </summary>
    /// <param name="model"></param>
    Task Create(CreateStudentPermissionWebViewModel model);
}