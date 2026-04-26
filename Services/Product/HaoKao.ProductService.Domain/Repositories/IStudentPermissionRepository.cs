using HaoKao.ProductService.Domain.Entities;

namespace HaoKao.ProductService.Domain.Repositories;

public interface IStudentPermissionRepository : IRepository<StudentPermission>
{
    /// <summary>
    /// 获取当前用户买过的产品id,名称和对应的协议id
    /// </summary>
    /// <returns></returns>
    Task<List<Tuple<Guid, string, Guid>>> GetAllAgreementId(Guid userId);

    Task<StudentPermission> GetWhereInclude(Guid userId, List<Guid> ids);

    Task<List<Guid>> GetProductIdsBy(Guid? subjectid, Guid? categoryId);

    /// <summary>
    /// 统计产品的购买人数
    /// </summary>
    /// <returns></returns>
    Task<Dictionary<Guid, int>> GetAllProductBuyerCount();

    /// <summary>
    /// 获取用户的有效权限
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<IReadOnlyList<StudentPermission>> GetEnabledAndNotExpiredByUserId(Guid userId);

    /// <summary>
    /// 把数据分入分表中
    /// </summary>
    /// <returns></returns>
    Task SplitDataToShareTable();
}