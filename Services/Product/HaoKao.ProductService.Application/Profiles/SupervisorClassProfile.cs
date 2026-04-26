using Girvs.AutoMapper;
using HaoKao.ProductService.Domain.Commands.SupervisorClass;
using HaoKao.ProductService.Domain.Entities;

namespace HaoKao.ProductService.Application.Profiles;

/// <summary>
/// 模型映射文件
/// </summary>
public class SupervisorClassProfile : Profile, IOrderedMapperProfile
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public SupervisorClassProfile()
    {
        CreateMap<CreateSupervisorClassCommand, SupervisorClass>();
        CreateMap<UpdateSupervisorClassCommand, SupervisorClass>();
    }
    /// <summary>
    /// 排序
    /// </summary>
    public int Order => 201;
}