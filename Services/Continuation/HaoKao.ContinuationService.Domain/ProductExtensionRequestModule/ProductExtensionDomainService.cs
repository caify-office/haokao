using HaoKao.ContinuationService.Domain.ProductExtensionPolicyModule;

namespace HaoKao.ContinuationService.Domain.ProductExtensionRequestModule;

public class ProductExtensionDomainService : IProductExtensionDomainService
{
    public bool CanMakeRequest(ProductExtensionPolicy policy, PolicyProduct product, List<ProductExtensionRequest> requests)
    {
        var productId = product.ProductId;

        // 1. 检查当前策略下是否已经成功申请 (每个策略只能成功一次)
        var isApprovedInCurrentPolicy = requests.Any(x => x.PolicyId == policy.Id &&
                                                          x.ProductId == productId &&
                                                          x.AuditState == ProductExtensionRequestState.Approved);

        if (isApprovedInCurrentPolicy) return false;

        // 2. 检查当前策略下是否有正在审核中的申请 (防止重复提交)
        var hasPending = requests.Any(x => x.PolicyId == policy.Id &&
                                           x.ProductId == productId &&
                                           x.AuditState == ProductExtensionRequestState.Waiting);

        if (hasPending) return false;

        // 3. 检查总权益是否已耗尽
        return CalculateRestCount(product, requests) > 0;
    }

    public int CalculateRestCount(PolicyProduct product, List<ProductExtensionRequest> requests)
    {
        var totalApprovedCount = requests.Count(x => x.ProductId == product.ProductId &&
                                                     x.AuditState == ProductExtensionRequestState.Approved);

        return Math.Max(0, product.MaxExtensionCount - totalApprovedCount);
    }
}