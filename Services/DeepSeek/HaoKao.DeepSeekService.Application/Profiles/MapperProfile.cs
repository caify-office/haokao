using Girvs.AutoMapper;
using HaoKao.DeepSeekService.Application.ViewModels;
using HaoKao.DeepSeekService.Domain;

namespace HaoKao.DeepSeekService.Application.Profiles;

public class MapperProfile : Profile, IOrderedMapperProfile
{
    /// <summary>
    /// 配置构造函数，用来创建关系映射
    /// </summary>
    public MapperProfile()
    {
        CreateMap<DeepSeekConfig, DeepSeekRequest>();
        CreateMap<Domain.ResponseFormat, ViewModels.ResponseFormat>();
        CreateMap<Domain.StreamOption, ViewModels.StreamOption>();
    }

    public int Order => 100;
}