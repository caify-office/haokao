using Girvs.AutoMapper;

namespace HaoKao.QuestionService.Domain.QuestionModule;

public class QuestionMapperProfile : Profile, IOrderedMapperProfile
{
    /// <summary>
    /// 配置构造函数，用来创建关系映射
    /// </summary>
    public QuestionMapperProfile()
    {
        CreateMap<CreateQuestionCommand, Question>();
        CreateMap<UpdateQuestionCommand, Question>();
    }

    public int Order => 100;
}