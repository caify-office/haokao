using HaoKao.ProductService.Application.ViewModels.Product;
using HaoKao.ProductService.Application.ViewModels.ProductPackage;

namespace HaoKao.ProductService.Application.Services.App;

public interface IProductPackageAppService : IAppWebApiService, IManager
{
    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    Task<BrowseProductPackageViewModel> Get(Guid id);

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    Task<QueryProductPackageViewModel> Get(QueryProductPackageViewModel queryViewModel);

    /// <summary>
    /// 通过产品id数组获取产品列表
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    Task<List<BrowseProductViewModel>> GetByIds(Guid[] ids);

    /// <summary>
    /// 通过产品包id数组获取产品包列表
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    Task<List<BrowseProductPackageViewModel>> GetProductPackageByIds([FromBody] Guid[] ids);

    /// <summary>
    /// 通过产品包id拿这些产品包下面所有产品的权限id
    /// </summary>
    /// <param name="ids">产品包ids</param>
    Task<IEnumerable<Guid>> GetProductPermissionId([FromBody] Guid[] ids);

    /// <summary>
    /// 验证当前用户在当前科目下是否具有有产品的权限
    /// </summary>
    /// <param name="Subjectid"></param>
    /// <param name="Categoryid"></param>
    /// <returns></returns>
    Task<bool> HasAuthority(Guid Subjectid, Guid Categoryid);
}