using HaoKao.QuestionService.Application.QuestionModule.ViewModels;
using HaoKao.QuestionService.Domain.QuestionCollectionModule;

namespace HaoKao.QuestionService.Application.QuestionCollectionModule.ViewModels;

[AutoMapFrom(typeof(QuestionCollectionQuery))]
[AutoMapTo(typeof(QuestionCollectionQuery))]
public class QueryQuestionCollectionViewModel : QueryDtoBase<QueryQuestionCollectionListViewModel>
{
    /// <summary>
    /// 科目Id
    /// </summary>
    public Guid? SubjectId { get; set; }

    /// <summary>
    /// 试题类型
    /// </summary>
    public Guid? QuestionTypeId { get; set; }
}

[AutoMapFrom(typeof(QuestionCollection))]
[AutoMapTo(typeof(QuestionCollection))]
public class QueryQuestionCollectionListViewModel : IDto
{
    /// <summary>
    /// 题目Id
    /// </summary>
    public Guid QuestionId { get; set; }

    /// <summary>
    /// 创建者Id，用户Id
    /// </summary>
    public Guid CreatorId { get; set; }

    /// <summary>
    /// 收藏时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 收藏的试题对象
    /// </summary>
    public BrowseQuestionViewModel Question { get; set; }
}