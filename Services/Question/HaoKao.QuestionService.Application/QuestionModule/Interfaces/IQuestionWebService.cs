using HaoKao.QuestionService.Application.QuestionModule.ViewModels;

namespace HaoKao.QuestionService.Application.QuestionModule.Interfaces;

public interface IQuestionWebService : IAppWebApiService, IManager
{
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
    /// 根据章节Id获取试题列表(包含节)
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<Dictionary<Guid, List<BrowseQuestionAppViewModel>>> GetChapterQuestionList(QueryChapterQuestionViewModel input);

    /// <summary>
    /// 根据小节Id获取试题列表(包含知识点)
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<Dictionary<Guid, List<BrowseQuestionAppViewModel>>> GetSectionQuestionList(QuerySectionQuestionViewModel input);

    /// <summary>
    /// 根据知识点Id获取试题列表(包含小节)
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<Dictionary<Guid, List<BrowseQuestionAppViewModel>>> GetKnowledgePointQuestionList(QueryKnowledgePointQuestionViewModel input);

    /// <summary>
    /// 根据试题Ids获取试卷的题目列表
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    Task<Dictionary<Guid, List<BrowseQuestionAppViewModel>>> GetPaperQuestionList(IReadOnlyList<Guid> ids);

    /// <summary>
    /// 按科目和能力抽取专项提升的试题
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<Dictionary<Guid, List<BrowseQuestionAppViewModel>>> GetSpecialPromotionQuestionList(QuerySpecialPromotionQuestionViewModel input);

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
    /// <param name="vm"></param>
    /// <returns></returns>
    /// <exception cref="GirvsException"></exception>
    Task<SubmitAnswerReturnViewModel> SubmitUserAnswers(SubmitAnswersViewModel vm);
}