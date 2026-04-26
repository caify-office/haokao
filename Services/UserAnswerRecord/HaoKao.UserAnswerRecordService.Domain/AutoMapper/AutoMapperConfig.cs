using Girvs.AutoMapper;
using HaoKao.UserAnswerRecordService.Domain.Commands;
using HaoKao.UserAnswerRecordService.Domain.Entities;

namespace HaoKao.UserAnswerRecordService.Domain.AutoMapper;

/// <summary>
/// 作答记录映射文件
/// </summary>
public class UserAnswerRecordProfile : Profile, IOrderedMapperProfile
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public UserAnswerRecordProfile()
    {
        CreateMap<EventCreateChapterAnswerRecordCommand, ChapterAnswerRecord>();
        CreateMap<EventCreatePaperAnswerRecordCommand, PaperAnswerRecord>();
        CreateMap<EventCreateDateAnswerRecordCommand, DateAnswerRecord>();
        CreateMap<EventCreateElapsedTimeRecordCommand, ElapsedTimeRecord>();

        CreateMap<EventCreateProductChapterAnswerRecordCommand, ProductChapterAnswerRecord>();
        CreateMap<EventCreateProductPaperAnswerRecordCommand, ProductPaperAnswerRecord>();
        CreateMap<EventCreateProductKnowledgeAnswerRecordCommand, ProductKnowledgeAnswerRecord>();
    }

    /// <summary>
    /// 排序
    /// </summary>
    public int Order => 201;
}