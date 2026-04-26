using Girvs.AutoMapper;
using HaoKao.AnsweringQuestionService.Application.ViewModels;
using HaoKao.AnsweringQuestionService.Application.ViewModels.AnsweringQuestion;
using HaoKao.AnsweringQuestionService.Application.ViewModels.AnsweringQuestionReply;
using HaoKao.AnsweringQuestionService.Domain.Entities;

namespace HaoKao.AnsweringQuestionService.Application.Profiles;

/// <summary>
/// 作答记录映射文件
/// </summary>
public class AnsweringQuestionProfile : Profile, IOrderedMapperProfile
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public AnsweringQuestionProfile()
    {
        //作答mapper
        CreateMap<AnsweringQuestionReply, BrowseAnsweringQuestionReplyViewModel>();
        //子级答疑转换
        CreateMap<AnsweringQuestion, BrowseAnsweringQuestionViewModel>();
    }
    /// <summary>
    /// 排序
    /// </summary>
    public int Order => 201;
}