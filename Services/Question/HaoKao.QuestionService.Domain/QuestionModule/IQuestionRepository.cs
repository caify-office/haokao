using Microsoft.EntityFrameworkCore.Query;

namespace HaoKao.QuestionService.Domain.QuestionModule;

public interface IQuestionRepository : IRepository<Question>
{

    Task<int> ExecuteUpdateAsync(Expression<Func<Question, bool>> predicate, Expression<Func<SetPropertyCalls<Question>, SetPropertyCalls<Question>>> setPropertyCalls);
    IQueryable<Question> Query { get; }

    Task<Dictionary<Guid, Question>> GetQuestionDictByIds(IEnumerable<Guid> ids);

    Task<string> GetQuestionOptionsByIdAsync(Guid id);

    Task<List<Question>> GetPaperSubQuestionListByParentIdsAsync(IEnumerable<Guid> parentIds);

    /// <summary>
    /// 按科目和分类查询试题数量
    /// </summary>
    /// <param name="subjectId"></param>
    /// <param name="categories"></param>
    /// <returns></returns>
    Task<int> GetSubjectQuestionCount(Guid subjectId, IReadOnlyList<Guid> categories);
    /// <summary>
    /// 按试题章节和试题分类查询试题数量
    /// </summary>
    /// <param name="chaperId"></param>
    /// <param name="questionCategoryId"></param>
    /// <returns></returns>
    public Task<int> GetChaperCategorieQuestionCount(Guid chaperId, Guid questionCategoryId);

    /// <summary>
    /// 根据科目代码抽取每日一题
    /// </summary>
    /// <param name="subjectId">科目Id</param>
    /// <param name="questionTypeIds">题型列表</param>
    /// <returns></returns>
    Task<Question> ExtractDailyQuestionBySubjectId(Guid subjectId, params Guid[] questionTypeIds);

    /// <summary>
    /// 根据Ids获取试题列表
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    Task<IReadOnlyList<Question>> GetQuestionListByIds(IReadOnlyList<Guid> ids);

    /// <summary>
    /// 按科目查询题库章节列表和题目数量
    /// </summary>
    /// <param name="subjectId">科目Id</param>
    /// <param name="categoryId">分类Id</param>
    /// <param name="freeState">是否免费</param>
    /// <returns></returns>
    Task<IReadOnlyList<(Guid Id, string Name, int Count)>> GetChapterList(Guid subjectId, Guid categoryId, FreeState? freeState);

    /// <summary>
    /// 按章节查询题库小节列表和题目数量
    /// </summary>
    /// <param name="chapterId">章节Id</param>
    /// <param name="categoryId">分类Id</param>
    /// <param name="freeState">是否免费</param>
    /// <returns></returns>
    Task<IReadOnlyList<(Guid Id, string Name, int Count)>> GetSectionList(Guid chapterId, Guid categoryId, FreeState? freeState);

    /// <summary>
    /// 按小节查询题库知识点列表和题目数量
    /// </summary>
    /// <param name="sectionId">小节Id</param>
    /// <param name="categoryId">分类Id</param>
    /// <param name="freeState">是否免费</param>
    /// <returns></returns>
    Task<IReadOnlyList<(Guid Id, string Name, int Count)>> GetKnowledgePointList(Guid sectionId, Guid categoryId, FreeState? freeState);

    /// <summary>
    /// 获取章节下的所有试题
    /// </summary>
    /// <param name="chapterId">章节Id</param>
    /// <param name="categoryId">分类Id</param>
    /// <param name="freeState">是否免费</param>
    /// <returns></returns>
    Task<IReadOnlyList<Question>> GetChapterQuestions(Guid chapterId, Guid categoryId, FreeState? freeState);

    /// <summary>
    /// 获取小节下的所有试题
    /// </summary>
    /// <param name="sectionId">小节Id</param>
    /// <param name="categoryId">分类Id</param>
    /// <param name="freeState">是否免费</param>
    /// <returns></returns>
    Task<IReadOnlyList<Question>> GetSectionQuestions(Guid sectionId, Guid categoryId, FreeState? freeState);

    /// <summary>
    /// 获取知识点下的所有试题
    /// </summary>
    /// <param name="knowledgePointId">知识点Id</param>
    /// <param name="categoryId">分类Id</param>
    /// <param name="freeState">是否免费</param>
    /// <returns></returns>
    Task<IReadOnlyList<Question>> GetKnowledgePointQuestions(Guid knowledgePointId, Guid categoryId, FreeState? freeState);

    /// <summary>
    /// 按科目和能力抽取专项提升的试题Ids
    /// </summary>
    /// <param name="subjectId">科目Id</param>
    /// <param name="ability">能力维度</param>
    /// <param name="count">数量</param>
    /// <param name="trial">试用</param>
    /// <returns></returns>
    Task<IReadOnlyList<Guid>> GetSpecialPromotionQuestionIds(Guid subjectId, string ability, int count, bool trial);

    /// <summary>
    /// 查询子题目返回合并结果集
    /// </summary>
    /// <param name="linq"></param>
    /// <returns></returns>
    Task<IReadOnlyList<Question>> GetListUnionChildren(IQueryable<Question> linq);
}