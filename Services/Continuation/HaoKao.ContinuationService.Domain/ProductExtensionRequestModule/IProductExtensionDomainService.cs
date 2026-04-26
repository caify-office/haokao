using Girvs.BusinessBasis;
using HaoKao.ContinuationService.Domain.ProductExtensionPolicyModule;

namespace HaoKao.ContinuationService.Domain.ProductExtensionRequestModule;

public interface IProductExtensionDomainService : IManager
{
    /// <summary>
    /// 判断是否可以申请续读
    /// </summary>
    /// <param name="policy">当前策略</param>
    /// <param name="product"></param>
    /// <param name="requests"></param>
    /// <returns>是否可申请</returns>
    bool CanMakeRequest(ProductExtensionPolicy policy, PolicyProduct product, List<ProductExtensionRequest> requests);

    /// <summary>
    /// 计算剩余可续读次数
    /// </summary>
    /// <param name="product"></param>
    /// <param name="requests">用户历史申请记录</param>
    /// <returns>剩余次数</returns>
    int CalculateRestCount(PolicyProduct product, List<ProductExtensionRequest> requests);
}