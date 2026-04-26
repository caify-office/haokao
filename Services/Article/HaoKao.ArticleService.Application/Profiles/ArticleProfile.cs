using AutoMapper;
using Girvs.AutoMapper;
using HaoKao.ArticleService.Domain.Entities;

namespace HaoKao.ArticleService.Application.Profiles;

/// <summary>
/// 文书模型映射文件
/// </summary>
public class ArticleProfile : Profile, IOrderedMapperProfile
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public ArticleProfile()
    {
        CreateMap<CreateArticleCommand, Article>();
        CreateMap<UpdateArticleCommand, Article>();
        CreateMap<SetArticleIsToppingCommand, Article>();
    }
    /// <summary>
    /// 排序
    /// </summary>
    public int Order => 201;
}