using Girvs.BusinessBasis.Queries;
using Girvs.Extensions.Collections;
using Girvs.Infrastructure;
using HaoKao.ProductService.Domain.Entities;
using HaoKao.ProductService.Domain.Enums;
using HaoKao.ProductService.Domain.Repositories;

namespace HaoKao.ProductService.Infrastructure.Repositories;

public class ProductRepository : Repository<Product>, IProductRepository
{
    public Task<Product> GetByIdInclude(Guid id)
    {
        return Queryable.Include(x => x.ProductPermissions)
                        .Include(x => x.AssistantProductPermissions)
                        .FirstOrDefaultAsync(x => x.Id.Equals(id));
    }

    public async Task<List<Product>> GetWhereInclude(Guid[] ids)
    {
        return await Queryable.Include(x => x.ProductPermissions)
                              .Include(x => x.AssistantProductPermissions)
                              .Where(x => ids.Contains(x.Id))
                              .ToListAsync();
    }

    public Task DeleteByIds(ICollection<Guid> ids)
    {
        return DeleteRangeAsync(x => ids.Contains(x.Id));
    }

    public Task UpdateEnableByIds(ICollection<Guid> ids, bool state)
    {
        return Queryable.Where(x => ids.Contains(x.Id))
                        .ExecuteUpdateAsync(s => s.SetProperty(x => x.Enable, state));
    }

    public Task UpdateIsShelvesByIds(ICollection<Guid> ids, bool state)
    {
        return Queryable.Where(x => ids.Contains(x.Id))
                        .ExecuteUpdateAsync(s => s.SetProperty(x => x.IsShelves, state));
    }

    public Task UpdateAnsweringByIds(IEnumerable<Guid> ids, bool answering)
    {
        return Queryable.Where(x => ids.Contains(x.Id))
                        .ExecuteUpdateAsync(s => s.SetProperty(x => x.Answering, answering));
    }

    /// <summary>
    /// 获取当前用户已过期，未过期和所有产品权限(包含过期时间和最早购买时间)
    /// </summary>
    /// <param name="userId"></param>
    /// <param name=""></param>
    /// <returns></returns>
    public async Task<List<Tuple<Product, DateTime, DateTime>>> GetMyAllProduct(ProductType? productType, Guid userId, PermissionExpiryType permissionExpiryType)
    {
        var tenantId = EngineContext.Current.ClaimManager.GetTenantId().ToGuid();

        var dbContext = EngineContext.Current.Resolve<ProductDbContext>();

        var myPermission = dbContext.StudentPermissions.Where(x => x.StudentId == userId && x.TenantId == tenantId && x.Enable == true);

        if (permissionExpiryType == PermissionExpiryType.Expired)
        {
            myPermission = myPermission.Where(x => x.ExpiryTime < DateTime.Now);
        }
        else if (permissionExpiryType == PermissionExpiryType.NotExpired)
        {
            myPermission = myPermission.Where(x => x.ExpiryTime >= DateTime.Now);
        }
        else if (permissionExpiryType == PermissionExpiryType.All)
        {
            // 获取当前用户所有产品
        }

        // 获取当前用户当前产品的最大过期时间
        // 用户可能对同一个产品有多条权限记录（例如多次购买续费）。
        var maxExpireQuery = myPermission
                             .GroupBy(x => new { x.StudentId, x.ProductId })
                             .Select(x => new
                             {
                                 x.Key.StudentId,
                                 x.Key.ProductId,
                                 MyExpireTime = x.Max(a => a.ExpiryTime), // 最大过期时间
                                 MyEarliestBuyTime = x.Min(i => i.CreateTime), // 最早购买时间
                             });

        var products = dbContext.Products.Include(x => x.ProductPermissions).Include(x => x.AssistantProductPermissions);

        var baseProductQuery = from permission in maxExpireQuery
                               from product in products
                               where permission.ProductId == product.Id
                               select new Tuple<Product, DateTime, DateTime>
                               (
                                   product,
                                   permission.MyExpireTime,
                                   permission.MyEarliestBuyTime
                               );

        var baseProduct = await baseProductQuery.ToListAsync();

        var baseProductId = baseProduct.Select(x => x.Item1.Id);

        var relateProductQuery = from rp in dbContext.RelatedProducts
                                                     .Where(x => baseProductId.Contains(x.RelatedTargetProductId)
                                                              && !baseProductId.Contains(x.ProductId))
                                 from p in dbContext.Products
                                                    .Include(x => x.ProductPermissions)
                                                    .Include(x => x.AssistantProductPermissions)
                                                    .Where(x => rp.ProductId == x.Id)
                                 select new Tuple<Product, DateTime, DateTime>(p, p.ExpiryTime, p.CreateTime);

        var relateProduct = await relateProductQuery.ToListAsync();

        var result = baseProduct.Union(relateProduct).ToList();

        if (productType.HasValue)
        {
            if (productType.Value is ProductType.Course or ProductType.IntelligentAssistanceCourse)
            {
                return result.Where(x => x.Item1.ProductType is ProductType.Course or ProductType.IntelligentAssistanceCourse).ToList();
            }
            return result.Where(x => x.Item1.ProductType == productType).ToList();
        }
        return result;
    }

    /// <summary>
    /// 获取我的产品所属科目
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="productType"></param>
    /// <returns></returns>
    public async Task<List<Tuple<Guid, string>>> GetMyProductSubject(Guid userId, ProductType? productType)
    {
        var dbContext = EngineContext.Current.Resolve<ProductDbContext>();

        //获取符合条件的产品
        var currentProduct = dbContext.Products.Include(x => x.ProductPermissions).AsNoTracking();

        var tenantId = EngineContext.Current.ClaimManager.GetTenantId()?.ToGuid();
        if (tenantId.HasValue)
        {
            currentProduct = currentProduct.Where(x => x.TenantId == tenantId);
        }
        if (productType.HasValue)
        {
            currentProduct = currentProduct.Where(x => x.ProductType == productType.Value);
        }

        // 获取当前用户当前产品的最大过期时间
        var myProduct = dbContext.StudentPermissions.Where(x => x.Enable == true
                                                             && x.StudentId == userId
                                                             && DateTime.Now.Date <= x.ExpiryTime.Date);
        //连接产品复合数据
        var result = from s in myProduct
                     from p in currentProduct.Where(p => p.Id == s.ProductId)
                     select p;

        return await result.SelectMany(x => x.ProductPermissions)
                           .GroupBy(x => new { x.SubjectId, x.SubjectName })
                           .Select(g => new Tuple<Guid, string>(
                                       g.Key.SubjectId,
                                       g.Key.SubjectName
                                   ))
                           .ToListAsync();
    }

    public override async Task<List<Product>> GetByQueryAsync(QueryBase<Product> query)
    {
        query.RecordCount = await Queryable.Where(query.GetQueryWhere()).CountAsync();

        if (query.RecordCount < 1)
        {
            query.Result = [];
        }
        else
        {
            query.Result =
                await Queryable.Where(query.GetQueryWhere())
                               .SelectProperties(query.QueryFields)
                               .OrderByDescending(x => x.CreateTime)
                               .Skip(query.PageStart)
                               .Take(query.PageSize)
                               .ToListAsync();
        }

        return query.Result;
    }

    /// <summary>
    /// 通过直播Id数组获取想关联的产品列表
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    public async Task<List<Product>> GetByLiveIdsAsync(IEnumerable<Guid> ids)
    {
        var dbContext = EngineContext.Current.Resolve<ProductDbContext>();
        var permissionsList = dbContext.ProductPermissions.Where(x => ids.Contains(x.PermissionId));
        var query = from product in dbContext.Products.Include(x => x.ProductPermissions)
                    from permissions in permissionsList.Where(permissions => product.Id == permissions.ProductId)
                    select product;

        var result = await query.ToListAsync();
        return result.GroupBy(x => x.Id).Select(x => x.First()).ToList();
    }

    public async Task<IReadOnlyList<Product>> GetWithGiftsAsync(Guid id)
    {
        var product = await Queryable.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        var giftIds = product.GiveAwayAList.Keys;
        var gifts = await Queryable.AsNoTracking().Where(x => giftIds.Contains(x.Id)).ToListAsync();
        return [product, .. gifts];
    }

    public async Task<IReadOnlyList<Product>> GetByIdsAsync(IEnumerable<Guid> ids)
    {
        return await Queryable.AsNoTracking().Where(x => ids.Contains(x.Id)).ToListAsync();
    }

    public async Task<IReadOnlyList<Product>> GetWithGiftsAsync(IEnumerable<Guid> ids)
    {
        var products = await Queryable.AsNoTracking().Where(x => ids.Contains(x.Id)).ToListAsync();
        var giftIds = products.SelectMany(x => x.GiveAwayAList.Keys);
        var gift = await Queryable.AsNoTracking().Where(x => giftIds.Contains(x.Id)).ToListAsync();
        return [..products, ..gift];
    }
}