using HaoKao.QuestionService.Application.QuestionModule.ViewModels;
using HaoKao.QuestionService.Domain.QuestionModule;

namespace HaoKao.QuestionService.Application.QuestionModule.Interfaces;

public interface IQuestionService : IAppWebApiService, IManager
{
    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    Task<BrowseQuestionViewModel> Get(Guid id);

    Task<Question> GetByIdAsync(Guid id);

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="viewModel">查询对象</param>
    Task<QueryQuestionViewModel> Get(QueryQuestionViewModel viewModel);

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="viewModel">查询对象</param>
    Task<QueryPaperViewModel> GetQuestionListForPaper(QueryPaperViewModel viewModel);

    /// <summary>
    /// 用于获取试卷题目列表
    /// </summary>
    Task<List<BrowsePaperViewModel>> GetPaperQuestionList([FromBody] IEnumerable<Guid> ids);

    /// <summary>
    /// 根据 案例分析题id集合 获取下面所有小题题干和id
    /// </summary>
    /// <param name="parentIds"></param>
    /// <returns></returns>
    Task<dynamic> GetPaperSubQuestionList([FromBody] IEnumerable<Guid> parentIds);

    /// <summary>
    /// 根据试题Id获取试题选项
    /// </summary>
    Task<dynamic> GetQuestionOptions(Guid id);

    /// <summary>
    /// 按试题章节和试题分类查询试题数量
    /// </summary>
    /// <param name="chapterId"></param>
    /// <param name="questionCategoryId"></param>
    /// <returns></returns>
    Task<int> GetChaperCategorieQuestionCount(Guid chapterId, Guid questionCategoryId);

    /// <summary>
    /// 创建试题实体
    /// </summary>
    /// <param name="model">新增模型</param>
    Task Create(CreateQuestionViewModel model);

    /// <summary>
    /// 根据主键删除指定试题实体
    /// </summary>
    Task Delete(IEnumerable<Guid> ids);

    /// <summary>
    /// 根据主键更新指定试题实体
    /// </summary>
    /// <param name="id">主键</param>
    /// <param name="model">更新模型</param>
    Task Update(UpdateQuestionViewModel model);

    /// <summary>
    /// 设为免费专区
    /// </summary>
    Task SetFreeState(IEnumerable<Guid> ids);

    /// <summary>
    /// 取消免费专区
    /// </summary>
    Task CancelFreeState(IEnumerable<Guid> ids);

    /// <summary>
    /// 启用
    /// </summary>
    Task EnableQuestion(IEnumerable<Guid> ids);

    /// <summary>
    /// 禁用
    /// </summary>
    Task DisableQuestion(IEnumerable<Guid> ids);

    /// <summary>
    /// 修改排序
    /// </summary>
    Task UpdateSort(Guid id, int sort);

    /// <summary>
    /// 根据科目获取试题的数量
    /// </summary>
    /// <param name="subjectId"></param>
    /// <returns></returns>
    Task<int> GetQuestionCount(Guid subjectId);

    /// <summary>
    /// 根据科目和分类获取章节列表和题目数量(包含节)
    /// </summary>
    /// <param name="input"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<IReadOnlyList<ChapterViewModel>> GetChapterList(QueryChapterViewModel input, Guid userId);
}