using HaoKao.SalespersonService.Domain.Commands;
using HaoKao.SalespersonService.Domain.Entities;

namespace HaoKao.SalespersonService.Application.Profiles;

public class EnterpriseWeChatConfigProfile : Profile, IOrderedMapperProfile
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public EnterpriseWeChatConfigProfile()
    {
        CreateMap<CreateEnterpriseWeChatConfigCommand, EnterpriseWeChatConfig>();
        CreateMap<UpdateEnterpriseWeChatConfigCommand, EnterpriseWeChatConfig>();
    }

    /// <summary>
    /// 排序
    /// </summary>
    public int Order => 201;
}