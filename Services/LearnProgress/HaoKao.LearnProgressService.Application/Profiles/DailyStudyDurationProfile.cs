using AutoMapper;
using Girvs.AutoMapper;
using HaoKao.LearnProgressService.Domain.Commands.DailyStudyDuration;
using HaoKao.LearnProgressService.Domain.Entities;

namespace HaoKao.LearnProgressService.Application.Profiles;

/// <summary>
/// 模型映射文件
/// </summary>
public class DailyStudyDurationProfile : Profile, IOrderedMapperProfile
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public DailyStudyDurationProfile()
    {
        CreateMap<CreateDailyStudyDurationCommand, DailyStudyDuration>();
    }
    /// <summary>
    /// 排序
    /// </summary>
    public int Order => 201;
}