using AutoMapper;
using Girvs.AutoMapper;
using HaoKao.ChapterNodeService.Domain.KnowledgePointModule;

namespace HaoKao.ChapterNodeService.Application.KnowledgePointModule.Profiles;

/// <summary>
/// 文书模型映射文件
/// </summary>
public class KnowledgePointProfile : Profile, IOrderedMapperProfile
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public KnowledgePointProfile()
    {
        CreateMap<CreateKnowledgePointCommand, KnowledgePoint>();
        CreateMap<UpdateKnowledgePointCommand, KnowledgePoint>();
    }
    /// <summary>
    /// 排序
    /// </summary>
    public int Order => 201;
}