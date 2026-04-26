using Girvs.AutoMapper;
using HaoKao.CourseService.Application.Modules.CourseChapterModule.ViewModels;
using HaoKao.CourseService.Domain.CourseChapterModule;

namespace HaoKao.CourseService.Application.Profiles;

/// <summary>
/// 模型映射文件
/// </summary>
public class CourseChapterProfile : Profile, IOrderedMapperProfile
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public CourseChapterProfile()
    {
        CreateMap<CreateCourseChapterViewModel, CourseChapter>();
        CreateMap<CreateCourseChapterCommand, CourseChapter>();
        CreateMap<UpdateCourseChapterCommand, CourseChapter>();
    }

    /// <summary>
    /// 排序
    /// </summary>
    public int Order => 201;
}