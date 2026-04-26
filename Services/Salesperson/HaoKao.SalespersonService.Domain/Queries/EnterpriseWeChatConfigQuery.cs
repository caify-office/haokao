using HaoKao.SalespersonService.Domain.Entities;

namespace HaoKao.SalespersonService.Domain.Queries;

public class EnterpriseWeChatConfigQuery : QueryBase<EnterpriseWeChatConfig>
{
    public override Expression<Func<EnterpriseWeChatConfig, bool>> GetQueryWhere()
    {
        return entity => true;
    }
}