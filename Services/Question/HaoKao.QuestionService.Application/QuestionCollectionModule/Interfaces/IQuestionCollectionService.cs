using HaoKao.QuestionService.Application.QuestionCollectionModule.ViewModels;

namespace HaoKao.QuestionService.Application.QuestionCollectionModule.Interfaces;

public interface IQuestionCollectionService : IManager
{
    /// <summary>
    /// 获取当前用户试题收藏分类统计
    /// </summary>
    /// <param name="subjectId"></param>
    /// <returns></returns>
    Task<List<QuestionCollectionStatViewModel>> Get(Guid subjectId);

    /// <summary>
    /// 获取收藏的试题列表，带分页、带条件查询
    /// </summary>
    /// <param name="viewModel"></param>
    /// <returns></returns>
    Task<QueryQuestionCollectionViewModel> Get(QueryQuestionCollectionViewModel viewModel);

    /// <summary>
    /// 是否收藏
    /// </summary>
    /// <param name="questionId"></param>
    /// <returns></returns>
    Task<bool> Any(Guid questionId);

    /// <summary>
    /// 创建试题收藏
    /// </summary>
    /// <param name="questionId">试题Id</param>
    Task Create(Guid questionId);

    /// <summary>
    /// 根据试题Id删除指定试题收藏
    /// </summary>
    /// <param name="questionId">试题Id</param>
    Task Delete(Guid questionId);
}