using HaoKao.BasicService.Domain.Entities;

namespace HaoKao.BasicService.Application.AutoMapperProfiles;

public class AuthorizeProfile : Profile, IOrderedMapperProfile
{
    public AuthorizeProfile()
    {
        CreateMap<AuthorizeDataRuleModel, UserRule>();
        CreateMap<UserRule, AuthorizeDataRuleModel>();


        CreateMap<AuthorizePermissionModel, BasalPermission>();
        CreateMap<BasalPermission, AuthorizePermissionModel>();
    }

    public int Order => 12;
}