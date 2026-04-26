using HaoKao.FeedBackService.Domain.Commands.Suggestion;
using HaoKao.FeedBackService.Domain.Entities;

namespace HaoKao.FeedBackService.Application.Profiles;

/// <summary>
/// 意见反馈映射文件
/// </summary>
public class SuggestionProfile : Profile, IOrderedMapperProfile
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public SuggestionProfile()
    {
        CreateMap<CreateSuggestionCommand, Suggestion>();
    }

    /// <summary>
    /// 排序
    /// </summary>
    public int Order => 201;
}