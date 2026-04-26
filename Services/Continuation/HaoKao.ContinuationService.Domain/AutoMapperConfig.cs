using HaoKao.ContinuationService.Domain.ContinuationAuditModule;
using HaoKao.ContinuationService.Domain.ContinuationSetupModule;
using HaoKao.ContinuationService.Domain.ProductExtensionPolicyModule;
using HaoKao.ContinuationService.Domain.ProductExtensionRequestModule;

namespace HaoKao.ContinuationService.Domain;

public class CommandToEntityMappingProfile : Profile, IOrderedMapperProfile
{
    /// <summary>
    /// 配置构造函数，用来创建关系映射
    /// </summary>
    public CommandToEntityMappingProfile()
    {
        CreateMap<CreateContinuationAuditCommand, ContinuationAudit>();
        CreateMap<UpdateContinuationAuditCommand, ContinuationAudit>();
        CreateMap<CreateContinuationSetupCommand, ContinuationSetup>();
        CreateMap<UpdateContinuationSetupCommand, ContinuationSetup>();

        CreateMap<CreateProductExtensionPolicyCommand, ProductExtensionPolicy>();
        CreateMap<UpdateProductExtensionPolicyCommand, ProductExtensionPolicy>();
        CreateMap<CreateProductExtensionRequestCommand, ProductExtensionRequest>();
    }

    public int Order => 100;
}