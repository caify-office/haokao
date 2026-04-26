using Girvs.AutoMapper;
using HaoKao.CourseService.Application.Modules.CoursePracticeModule.ViewModels;
using HaoKao.CourseService.Domain.CoursePracticeModule;

namespace HaoKao.CourseService.Application.Profiles;

/// <summary>
/// 模型映射文件
/// </summary>
public class CoursePracticeProfile : Profile, IOrderedMapperProfile
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public CoursePracticeProfile()
    {
        CreateMap<CreateCoursePracticeCommand, CoursePractice>();
        CreateMap<UpdateCoursePracticeCommand, CoursePractice>();
        CreateMap<SaveAssistantCoursePracticeCommand, CoursePractice>();
        CreateMap<CreateCoursePracticeCommand, CoursePractice>();
        CreateMap<UpdateCoursePracticeCommand, CoursePractice>();
        CreateMap<SaveCoursePracticeViewModel, CreateCoursePracticeViewModel>();
        CreateMap<SaveCoursePracticeViewModel, UpdateCoursePracticeViewModel>();
    }

    /// <summary>
    /// 排序
    /// </summary>
    public int Order => 201;
}