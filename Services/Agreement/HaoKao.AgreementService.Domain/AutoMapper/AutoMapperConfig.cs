using HaoKao.AgreementService.Domain.Commands;
using HaoKao.AgreementService.Domain.Entities;

namespace HaoKao.AgreementService.Domain.AutoMapper;

/// <summary>
/// 静态全局 AutoMapper 配置文件
/// </summary>
public class AutoMapperConfig
{
    public static MapperConfiguration RegisterMappings()
    {
        return new MapperConfiguration(cfg => { cfg.AddProfile(new CommandToEntityMappingProfile()); });
    }
}

public class CommandToEntityMappingProfile : Profile, IOrderedMapperProfile
{
    /// <summary>
    /// 配置构造函数，用来创建关系映射
    /// </summary>
    public CommandToEntityMappingProfile()
    {
        CreateMap<CreateCourseAgreementCommand, CourseAgreement>();
        CreateMap<UpdateCourseAgreementCommand, CourseAgreement>();
    }

    public int Order => 100;
}