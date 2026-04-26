using HaoKao.QuestionService.Application.QuestionModule.ViewModels;

namespace HaoKao.QuestionService.Application.QuestionModule.Interfaces;

public interface IQuestionWeChatService : IAppWebApiService, IManager
{
    /// <summary>
    /// 根据主键获取题目和收藏状态
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<BrowseQuestionAppViewModel> Get(Guid id);

    /// <summary>
    /// 查询科目下试题总数
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<int> GetSubjectQuestionCount(QuerySubjectQuestionCountViewModel input);

    /// <summary>
    /// 根据科目和分类获取章节列表和题目数量
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<IReadOnlyList<ChapterViewModel>> GetChapterList(QueryChapterViewModel input);

    /// <summary>
    /// 根据章节和分类获取小节列表和题目数量
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<IReadOnlyList<ChapterViewModel>> GetSectionList(QuerySectionViewModel input);

    /// <summary>
    /// 根据小节和分类获取知识点列表和题目数量
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<IReadOnlyList<ChapterViewModel>> GetKnowledgePointList(QueryKnowledgePointViewModel input);

    /// <summary>
    /// 获取章节试题Id, 并根据题型进行分组
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<Dictionary<Guid, List<QueryQuestionListAppViewModel>>> GetChapterQuestionIdGroup(QueryChapterQuestionViewModel input);

    /// <summary>
    /// 获取小节试题Id, 并根据题型进行分组
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<Dictionary<Guid, List<QueryQuestionListAppViewModel>>> GetSectionQuestionIdGroup(QuerySectionQuestionViewModel input);

    /// <summary>
    /// 获取知识点试题Id, 并根据题型进行分组
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<Dictionary<Guid, List<QueryQuestionListAppViewModel>>> GetKnowledgePointQuestionIdGroup(QueryKnowledgePointQuestionViewModel input);

    /// <summary>
    /// 获取试卷试题Id, 并根据题型进行分组
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    Task<Dictionary<Guid, List<QueryQuestionListAppViewModel>>> GetPaperQuestionIdGroup(IReadOnlyList<Guid> ids);

    /// <summary>
    /// 按科目和能力抽取专项提升的试题Id, 并根据题型进行分组
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<Dictionary<Guid, List<QueryQuestionListAppViewModel>>> GetSpecialPromotionQuestionIdGroup(QuerySpecialPromotionQuestionViewModel input);

    /// <summary>
    /// 章节下是否存在免费试题
    /// </summary>
    /// <param name="subjectId"></param>
    /// <param name="chapterId"></param>
    /// <param name="categoryId"></param>
    /// <returns></returns>
    Task<bool> ExistsFreeQuestion(Guid subjectId, Guid chapterId, Guid categoryId);

    /// <summary>
    /// 用户提交作答
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    /// <exception cref="GirvsException"></exception>
    Task<SubmitAnswerReturnViewModel> SubmitUserAnswers(SubmitAnswersViewModel input);
}