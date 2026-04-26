using HaoKao.StudentService.Domain.Commands;
using HaoKao.StudentService.Domain.Entities;

namespace HaoKao.StudentService.Application.Profiles;

public class AutoMapperProfile : Profile, IOrderedMapperProfile
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public AutoMapperProfile()
    {
        CreateMap<CreateStudentParameterConfigCommand, StudentParameterConfig>();
        CreateMap<UpdateStudentParameterConfigCommand, StudentParameterConfig>();
        CreateMap<SaveStudentParameterConfigCommand, StudentParameterConfig>();
    }

    /// <summary>
    /// 排序
    /// </summary>
    public int Order => 201;
}