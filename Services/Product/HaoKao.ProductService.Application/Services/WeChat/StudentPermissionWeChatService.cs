using HaoKao.ProductService.Application.Services.Web;
using HaoKao.ProductService.Application.ViewModels.StudentPermission;
using HaoKao.ProductService.Domain.Enums;

namespace HaoKao.ProductService.Application.Services.WeChat;

/// <summary>
/// 学生权限WeChat端接口
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.WeChat)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class StudentPermissionWeChatService(IStudentPermissionWebService webService) : IStudentPermissionWeChatService
{
    /// <summary>
    /// 获取当前用户买过的产品id,名称和对应的协议id
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public Task<List<AgreementListViewModel>> GetAllAgreement()
    {
        return webService.GetAllAgreement();
    }

    /// <summary>
    /// 根据产品Id获取用户权限
    /// </summary>
    /// <param name="productId">产品Id</param>
    [HttpGet("{productId:guid}")]
    public Task<BrowseStudentPermissionViewModel> GetByProductId(Guid productId)
    {
        return webService.GetByProductId(productId);
    }

    /// <summary>
    /// 验证当前用户在当前科目下是否具有有产品的权限
    /// </summary>
    /// <param name="productType">产品类型</param>
    /// <param name="subjectId">科目id</param>
    /// <param name="premissionId">权限id</param>
    /// <returns></returns>
    [HttpGet]
    public Task<bool> HasAuthority(ProductType productType, Guid? subjectId, Guid? premissionId)
    {
        return webService.HasAuthority(productType, subjectId, premissionId);
    }

    /// <summary>
    /// 获取关联当前权限Id的产品Id
    /// </summary>
    /// <param name="subjectId">科目id</param>
    /// <param name="premissionId">权限id</param>
    /// <returns></returns>
    [HttpGet]
    public Task<List<Guid>> GetProductIds(Guid? subjectId, Guid? premissionId)
    {
        return webService.GetProductIds(subjectId, premissionId);
    }

    /// <summary>
    /// 判断是否有当前产品的权限
    /// </summary>
    /// <param name="productType">产品类别</param>
    /// <param name="productId">产品Id</param>
    [HttpGet]
    public Task<bool> HasProductAuthority(ProductType productType, Guid productId)
    {
        return webService.HasProductAuthority(productType, productId);
    }

    /// <summary>
    /// 判断是否有当前直播的权限
    /// </summary>
    /// <param name="liveId">直播Id</param>
    [HttpGet]
    public Task<bool> HasLiveAuthority(Guid liveId)
    {
        return webService.HasLiveAuthority(liveId);
    }

    /// <summary>
    /// 创建学员权限
    /// </summary>
    /// <param name="model"></param>
    [HttpPost]
    public Task Create([FromBody] CreateStudentPermissionWebViewModel model)
    {
        return webService.Create(model);
    }
}