using Girvs.AutoMapper;

namespace HaoKao.CourseService.Domain.CourseVideoNoteModule;

/// <summary>
/// 模型映射文件
/// </summary>
public class CourseVideoNoteProfile : Profile, IOrderedMapperProfile
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public CourseVideoNoteProfile()
    {
        CreateMap<CreateCourseVideoNoteCommand, CourseVideoNote>();
        CreateMap<UpdateCourseVideoNoteCommand, CourseVideoNote>();
    }

    /// <summary>
    /// 排序
    /// </summary>
    public int Order => 201;
}