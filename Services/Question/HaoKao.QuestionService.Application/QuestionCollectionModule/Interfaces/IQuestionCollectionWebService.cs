using HaoKao.QuestionService.Application.QuestionCollectionModule.ViewModels;
using HaoKao.QuestionService.Application.QuestionModule.ViewModels;

namespace HaoKao.QuestionService.Application.QuestionCollectionModule.Interfaces;

public interface IQuestionCollectionWebService : IAppWebApiService, IManager
{
    /// <summary>
    /// 获取当前用户试题收藏分类统计
    /// </summary>
    /// <returns></returns>
    Task<List<QuestionCollectionStatViewModel>> Get(Guid subjectId);

    /// <summary>
    /// 根据科目和题型获取收藏的试题
    /// </summary>
    /// <param name="subjectId"></param>
    /// <param name="typeId"></param>
    /// <returns></returns>
    Task<Dictionary<Guid, List<BrowseQuestionAppViewModel>>> Get(Guid subjectId, Guid? typeId);

    /// <summary>
    /// 收藏试题
    /// </summary>
    /// <param name="questionId"></param>
    /// <returns></returns>
    Task<bool> Create(Guid questionId);

    /// <summary>
    /// 取消收藏试题
    /// </summary>
    /// <param name="questionId"></param>
    /// <returns></returns>
    Task<bool> Delete(Guid questionId);

    /// <summary>
    /// 试题列表添加是否收藏状态
    /// </summary>
    /// <param name="list"></param>
    Task SetIsCollected(IReadOnlyCollection<BrowseQuestionAppViewModel> list);
}