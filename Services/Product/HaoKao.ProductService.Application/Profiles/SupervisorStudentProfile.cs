using Girvs.AutoMapper;
using HaoKao.ProductService.Domain.Commands.SupervisorStudent;
using HaoKao.ProductService.Domain.Entities;

namespace HaoKao.ProductService.Application.Profiles;

/// <summary>
/// 模型映射文件
/// </summary>
public class SupervisorStudentProfile : Profile, IOrderedMapperProfile
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public SupervisorStudentProfile()
    {
        CreateMap<CreateSupervisorStudentCommand, SupervisorStudent>();
        CreateMap<UpdateSupervisorStudentCommand, SupervisorStudent>();
    }
    /// <summary>
    /// 排序
    /// </summary>
    public int Order => 201;
}