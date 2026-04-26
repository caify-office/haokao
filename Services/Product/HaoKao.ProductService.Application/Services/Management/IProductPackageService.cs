using HaoKao.ProductService.Application.ViewModels.Product;
using HaoKao.ProductService.Application.ViewModels.ProductPackage;

namespace HaoKao.ProductService.Application.Services.Management;

public interface IProductPackageService : IAppWebApiService, IManager
{
    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    Task<BrowseProductPackageViewModel> Get(Guid id);

    /// <summary>
    /// 通过产品包id数组获取产品列表
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    Task<List<ProductListViewModel>> GetProducts(Guid id);

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
    /// 创建产品包
    /// </summary>
    /// <param name="model">新增模型</param>
    Task Create(CreateProductPackageViewModel model);

    /// <summary>
    /// 根据主键删除指定产品包
    /// </summary>
    /// <param name="id">主键</param>
    Task Delete(Guid id);

    /// <summary>
    /// 根据主键更新指定产品包
    /// </summary>
    /// <param name="id">主键</param>
    /// <param name="model">新增模型</param>
    Task Update(Guid id, UpdateProductPackageViewModel model);

    /// <summary>
    /// 启用
    /// </summary>
    /// <param name="id">主键</param>
    Task Enable(Guid id);

    /// <summary>
    /// 禁用
    /// </summary>
    /// <param name="id">主键</param>
    Task DisEnable(Guid id);

    /// <summary>
    /// 上架
    /// </summary>
    /// <param name="id">主键</param>
    Task ShelvesUp(Guid id);

    /// <summary>
    /// 下架
    /// </summary>
    /// <param name="id">主键</param>
    Task ShelvesDown(Guid id);

    /// <summary>
    /// 设置产品
    /// </summary>
    /// <param name="id">主键</param>
    /// <param name="model"></param>
    Task ProductList(Guid id, SetProductPackageProductListViewModel model);
}