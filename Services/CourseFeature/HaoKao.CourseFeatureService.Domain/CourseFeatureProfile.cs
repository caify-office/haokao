namespace HaoKao.CourseFeatureService.Domain;

public class CourseFeatureProfile : Profile, IOrderedMapperProfile
{
    /// <summary>
    /// 配置构造函数，用来创建关系映射
    /// </summary>
    public CourseFeatureProfile()
    {
        CreateMap<CreateCourseFeatureCommand, CourseFeature>();
        CreateMap<UpdateCourseFeatureCommand, CourseFeature>();
    }

    public int Order => 100;
}