using HaoKao.FeedBackService.Application.ViewModels.FeedBack;
using HaoKao.FeedBackService.Application.ViewModels.FeedBackReply;
using HaoKao.FeedBackService.Domain.Entities;

namespace HaoKao.FeedBackService.Application.Profiles;

/// <summary>
/// 用户答疑映射文件
/// </summary>
public class FeedBackProfile : Profile, IOrderedMapperProfile
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public FeedBackProfile()
    {
        //作答mapper
        CreateMap<FeedBackReply, BrowseFeedBackReplyViewModel>();
        //子级答疑转换
        CreateMap<FeedBack, BrowseFeedBackViewModel>();
    }

    /// <summary>
    /// 排序
    /// </summary>
    public int Order => 201;
}