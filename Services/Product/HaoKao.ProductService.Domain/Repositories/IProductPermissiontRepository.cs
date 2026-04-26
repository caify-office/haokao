using HaoKao.ProductService.Domain.Entities;

namespace HaoKao.ProductService.Domain.Repositories;

public interface IProductPermissionRepository : IRepository<ProductPermission>
{
    /// <summary>
    /// 查询复合条件的产品id
    /// </summary>
    /// <param name="subjectid"></param>
    /// <param name="permissionId"></param>
    /// <returns></returns>
    Task<List<Guid>> GetProductIdsBy(Guid? subjectid, Guid? permissionId);
}