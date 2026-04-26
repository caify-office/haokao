using HaoKao.ProductService.Application.ViewModels.ProductPackage;
using HaoKao.ProductService.Domain.Repositories;

namespace HaoKao.ProductService.Application.Services.Web;

public interface IProductPackageWebService : IAppWebApiService,IManager
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
    /// 通过产品包id数组获取每个产品包的复合数据
    /// </summary>
    /// <param name="ids"></param>
    Task<List<ProductPackageCompositeAttribute>> GetCompositeAttributeByIds([FromBody] List<Guid> ids);
}