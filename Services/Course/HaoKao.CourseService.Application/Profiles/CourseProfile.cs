using Girvs.AutoMapper;
using HaoKao.CourseService.Domain.CourseModule;

namespace HaoKao.CourseService.Application.Profiles;

/// <summary>
/// 模型映射文件
/// </summary>
public class CourseProfile : Profile, IOrderedMapperProfile
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public CourseProfile()
    {
        CreateMap<CreateCourseCommand, Course>();
        CreateMap<UpdateCourseCommand, Course>();
        CreateMap<UpdateCourseMaterialsPackageUrlCommand, Course>();
    }

    /// <summary>
    /// 排序
    /// </summary>
    public int Order => 201;
}