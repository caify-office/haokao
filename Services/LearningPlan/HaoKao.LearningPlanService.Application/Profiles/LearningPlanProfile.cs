using AutoMapper;
using Girvs.AutoMapper;

namespace HaoKao.LearningPlanService.Application.Profiles;

/// <summary>
/// 模型映射文件
/// </summary>
public class LearningPlanProfile : Profile, IOrderedMapperProfile
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public LearningPlanProfile()
    {
        CreateMap<CreateLearningPlanCommand, LearningPlan>();
    }

    /// <summary>
    /// 排序
    /// </summary>
    public int Order => 201;
}