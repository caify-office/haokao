using HaoKao.UserAnswerRecordService.Application.Helpers;
using HaoKao.UserAnswerRecordService.Application.Modules.AnswerRecordModule;
using HaoKao.UserAnswerRecordService.Application.Modules.UserAnswerRecordModule.ViewModels;
using HaoKao.UserAnswerRecordService.Domain.Entities;

namespace HaoKao.UserAnswerRecordService.Application.AutoMapper;

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
        CreateMap<UserAnswerRecord, UserAnswerRecordAppViewModel>();
        CreateMap<UserAnswerQuestion, UserAnswerRecordQuestionAppViewModel>();
        CreateMap<UserQuestionOption, UserAnswerRecordQuestionOptionAppViewModel>();
    }

    /// <summary>
    /// 排序
    /// </summary>
    public int Order => 201;
}

/// <summary>
/// 作答记录映射文件
/// </summary>
public class AnswerRecordMapperProfile : Profile, IOrderedMapperProfile
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public AnswerRecordMapperProfile()
    {
        CreateMap<ChapterAnswerRecord, ChapterRecordViewModel>()
            .ForMember(x => x.QuestionCount, x => x.MapFrom(src => src.AnswerRecord.QuestionCount))
            .ForMember(x => x.AnswerCount, x => x.MapFrom(src => src.AnswerRecord.AnswerCount))
            .ForMember(x => x.CorrectCount, x => x.MapFrom(src => src.AnswerRecord.CorrectCount));

        CreateMap<ChapterAnswerRecord, SectionRecordViewModel>()
            .ForMember(x => x.QuestionCount, x => x.MapFrom(src => src.AnswerRecord.QuestionCount))
            .ForMember(x => x.AnswerCount, x => x.MapFrom(src => src.AnswerRecord.AnswerCount))
            .ForMember(x => x.CorrectCount, x => x.MapFrom(src => src.AnswerRecord.CorrectCount));

        CreateMap<ChapterAnswerRecord, KnowledgePointRecordViewModel>()
            .ForMember(x => x.QuestionCount, x => x.MapFrom(src => src.AnswerRecord.QuestionCount))
            .ForMember(x => x.AnswerCount, x => x.MapFrom(src => src.AnswerRecord.AnswerCount))
            .ForMember(x => x.CorrectCount, x => x.MapFrom(src => src.AnswerRecord.CorrectCount));

        CreateMap<PaperAnswerRecord, PaperRecordViewModel>()
            .ForMember(x => x.CorrectRate, x => x.MapFrom(src => PercentageHelper.CalculatePercentage(src.AnswerRecord.CorrectCount, src.AnswerRecord.QuestionCount)));

        CreateMap<PaperAnswerRecord, PaperAnswerRecordViewModel>()
            .ForMember(x => x.QuestionCount, x => x.MapFrom(src => src.AnswerRecord.QuestionCount))
            .ForMember(x => x.AnswerCount, x => x.MapFrom(src => src.AnswerRecord.AnswerCount))
            .ForMember(x => x.CorrectCount, x => x.MapFrom(src => src.AnswerRecord.CorrectCount))
            .ForMember(x => x.Questions, x => x.MapFrom(src => src.AnswerRecord.Questions))
            .ForMember(x => x.CorrectRate, x => x.MapFrom(src => PercentageHelper.CalculatePercentage(src.AnswerRecord.CorrectCount, src.AnswerRecord.QuestionCount)));
    }

    /// <summary>
    /// 排序
    /// </summary>
    public int Order => 201;
}