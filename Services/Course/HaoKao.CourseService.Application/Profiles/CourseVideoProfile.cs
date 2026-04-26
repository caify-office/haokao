using Girvs.AutoMapper;
using HaoKao.CourseService.Application.Modules.CourseVideoModule.ViewModels;
using HaoKao.CourseService.Domain.CourseVideoModule;

namespace HaoKao.CourseService.Application.Profiles;

/// <summary>
/// 模型映射文件
/// </summary>
public class CourseVideoProfile : Profile, IOrderedMapperProfile
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public CourseVideoProfile()
    {
        CreateMap<SaveCourseVideoViewModel, CourseVideo>();
        CreateMap<SaveCourseVideoCommand, CourseVideo>();
        CreateMap<UpdateCourseVideoCommand, CourseVideo>();
    }

    /// <summary>
    /// 排序
    /// </summary>
    public int Order => 201;
}