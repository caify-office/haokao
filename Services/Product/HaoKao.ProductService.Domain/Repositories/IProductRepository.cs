using HaoKao.ProductService.Domain.Entities;
using HaoKao.ProductService.Domain.Enums;

namespace HaoKao.ProductService.Domain.Repositories;

public interface IProductRepository : IRepository<Product>
{
    Task<Product> GetByIdInclude(Guid id);

    Task<List<Product>> GetWhereInclude(Guid[] ids);

    Task DeleteByIds(ICollection<Guid> ids);

    Task UpdateEnableByIds(ICollection<Guid> ids, bool state);

    Task UpdateIsShelvesByIds(ICollection<Guid> ids, bool state);

    Task UpdateAnsweringByIds(IEnumerable<Guid> ids, bool answering);

    Task<List<Tuple<Product, DateTime, DateTime>>> GetMyAllProduct(ProductType? productType, Guid userId, PermissionExpiryType permissionExpiryType);

    /// <summary>
    /// 获取我的产品所属科目
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="productType"></param>
    /// <returns></returns>
    Task<List<Tuple<Guid, string>>> GetMyProductSubject(Guid userId, ProductType? productType);

    /// <summary>
    /// 通过直播Id数组获取产品列表
    /// </summary>
    /// <param name="ids">直播Id数组</param>
    /// <returns></returns>
    Task<List<Product>> GetByLiveIdsAsync(IEnumerable<Guid> ids);

    /// <summary>
    /// 通过产品Id获取产品和赠品列表
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<IReadOnlyList<Product>> GetWithGiftsAsync(Guid id);

    /// <summary>
    /// 通过产品Id数组获取产品和赠品列表
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    Task<IReadOnlyList<Product>> GetByIdsAsync(IEnumerable<Guid> ids);

    /// <summary>
    /// 通过产品Id数组获取产品和赠品列表(需要查赠品)
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    Task<IReadOnlyList<Product>> GetWithGiftsAsync(IEnumerable<Guid> ids);
}