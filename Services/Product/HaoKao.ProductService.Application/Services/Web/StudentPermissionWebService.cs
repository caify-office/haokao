using HaoKao.ProductService.Application.Services.Management;
using HaoKao.ProductService.Application.ViewModels.StudentPermission;
using HaoKao.ProductService.Domain.Commands.StudentPermission;
using HaoKao.ProductService.Domain.Entities;
using HaoKao.ProductService.Domain.Enums;
using HaoKao.ProductService.Domain.Repositories;
using Newtonsoft.Json;

namespace HaoKao.ProductService.Application.Services.Web;

/// <summary>
/// 学生权限web端接口
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.WebSite)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class StudentPermissionWebService(
    IStudentPermissionRepository repository,
    IStaticCacheManager cacheManager,
    IMediatorHandler bus,
    INotificationHandler<DomainNotification> notifications,
    IProductWebService productWebService,
    IProductService productService,
    IProductRepository productRepository
) : IStudentPermissionWebService
{
    private readonly DomainNotificationHandler _notifications = (DomainNotificationHandler)notifications;

    /// <summary>
    /// 获取当前用户买过的产品id,名称和对应的协议id
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<List<AgreementListViewModel>> GetAllAgreement()
    {
        var userId = EngineContext.Current.ClaimManager.GetUserId().ToGuid();
        var result = await repository.GetAllAgreementId(userId);

        return result.Select(x => new AgreementListViewModel
        {
            ProductId = x.Item1,
            ProductName = x.Item2,
            AgreementId = x.Item3,
        }).ToList();
    }

    /// <summary>
    /// 根据产品Id获取用户权限
    /// </summary>
    /// <param name="productId">产品Id</param>
    [HttpGet("{productId:guid}")]
    public async Task<BrowseStudentPermissionViewModel> GetByProductId(Guid productId)
    {
        var userId = EngineContext.Current.ClaimManager.GetUserId().To<Guid>();
        var key = $"studentId={userId}&productId={productId}".ToMd5();
        var entity = await cacheManager.GetAsync(
            GirvsEntityCacheDefaults<StudentPermission>.QueryCacheKey.Create(key),
            async () => await repository.GetAsync(x => x.StudentId == userId && x.ProductId == productId)
        );
        // var entity = await repository.GetAsync(x => x.StudentId == userId && x.ProductId == productId);
        return entity.MapToDto<BrowseStudentPermissionViewModel>();
    }

    /// <summary>
    /// 验证当前用户在当前科目下是否具有有产品的权限
    /// </summary>
    /// <param name="productType">产品类型</param>
    /// <param name="subjectId">科目id</param>
    /// <param name="premissionId">权限id</param>
    /// <returns></returns>
    [HttpGet]
    public async Task<bool> HasAuthority(ProductType productType, Guid? subjectId, Guid? premissionId)
    {
        //写sql查询出科目下面的产品ids
        var productIds = await productService.GetProductIds(subjectId, premissionId);
        //验证取到的产品ids是否在myProduct中是否存在
        var userProducts = await productWebService.GetMyProduct(productType);
        return productIds.Any(x => userProducts.Select(p => p.Id).Contains(x));
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
        return productService.GetProductIds(subjectId, premissionId);
    }

    /// <summary>
    /// 判断是否有当前产品的权限
    /// </summary>
    /// <param name="productType">产品类别</param>
    /// <param name="productId">产品Id</param>
    [HttpGet]
    public async Task<bool> HasProductAuthority(ProductType productType, Guid productId)
    {
        var userProducts = await productWebService.GetMyProduct(productType);
        return userProducts.Any(x => x.Id == productId);
    }

    /// <summary>
    /// 判断是否有当前直播的权限
    /// </summary>
    /// <param name="liveId">直播Id</param>
    [HttpGet]
    public async Task<bool> HasLiveAuthority(Guid liveId)
    {
        var userProducts = await productWebService.GetMyProduct(ProductType.Live);
        var productPermissionList = userProducts.SelectMany(x => x.ProductPermissions).ToList();
        return productPermissionList.Any(x => x.PermissionId == liveId);
    }

    /// <summary>
    /// 创建学员权限
    /// </summary>
    /// <param name="model"></param>
    [HttpPost]
    public async Task Create([FromBody] CreateStudentPermissionWebViewModel model)
    {
        var purchaseProductContentList = JsonConvert.DeserializeObject<List<PurchaseProductContent>>(model.PurchaseProductContents);
        var productIds = purchaseProductContentList.Select(x => x.ContentId).ToList();
        var products = await productRepository.GetWhereAsync(x => productIds.Contains(x.Id));
        if (products.Any(x =>x.ExpiryTimeTypeEnum==Common.Enums.ExpiryTimeTypeEnum.Date&& x.ExpiryTime<DateTime.Now))
        {
            throw new GirvsException("存在过期的学员权限");
        }
        if (products.Any(x => x.DiscountedPrice > 0))
        {
            throw new GirvsException("不是免费产品");
        }

        var command = new CreateStudentPermissionEventCommand(
            model.StudentName,
            EngineContext.Current.ClaimManager.GetUserId().To<Guid>(),
            "",
            model.PurchaseProductContents,
            model.SourceMode
        );

        await bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }
}