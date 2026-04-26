using HaoKao.ProductService.Domain.Entities;
using HaoKao.ProductService.Domain.Repositories;

namespace HaoKao.ProductService.Infrastructure.Repositories;

public class ProductPermissionRepository : Repository<ProductPermission>, IProductPermissionRepository
{
    /// <summary>
    /// 查询复合条件的产品id
    /// </summary>
    /// <param name="subjectid"></param>
    /// <param name="permissionId"></param>
    /// <returns></returns>
    public Task<List<Guid>> GetProductIdsBy(Guid? subjectid, Guid? permissionId)
    {
        return Queryable.Where(x => subjectid.HasValue || x.SubjectId == subjectid.Value)
                        .Where(x => permissionId.HasValue || x.PermissionId == permissionId.Value)
                        .Select(x => x.ProductId).Distinct().ToListAsync();
    }
}