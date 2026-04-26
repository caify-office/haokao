using Girvs.AutoMapper;
using HaoKao.CourseService.Domain.CourseMaterialsModule;

namespace HaoKao.CourseService.Application.Profiles;

/// <summary>
/// 模型映射文件
/// </summary>
public class CourseMaterialsProfile : Profile, IOrderedMapperProfile
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public CourseMaterialsProfile()
    {
        CreateMap<CreateCourseMaterialsCommand, CourseMaterials>();
        CreateMap<SaveCourseMaterialsCommand, CourseMaterials>();
    }

    /// <summary>
    /// 排序
    /// </summary>
    public int Order => 201;
}