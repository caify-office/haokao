using HaoKao.CourseService.Application.Modules.CoursePracticeModule.ViewModels;

namespace HaoKao.CourseService.Application.Modules.CoursePracticeModule.Interfaces;

public interface ICoursePracticeWebService : IAppWebApiService, IManager
{
    /// <summary>
    /// 根据课程章节id获取指定
    /// </summary>
    /// <param name="courseChapterId">课程章节id</param>
    Task<BrowseCoursePracticeViewModel> Get(Guid courseChapterId);

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    Task<QueryCoursePracticeViewModel> Get(QueryCoursePracticeViewModel queryViewModel);

    /// <summary>
    /// 根据课程id和章节id获取指定章节练习(智辅课程专用)
    /// </summary>
    /// <param name="courseId">课程id</param>
    /// <param name="courseChapterId">Id</param>
    Task<BrowseCoursePracticeViewModel> GetChapterPractice(Guid courseId, Guid courseChapterId);

    /// <summary>
    /// 通过知识点Id获取关联知识点对应的课后练习
    /// </summary>
    /// <param name="courseId">课程id</param>
    /// <param name="knowledgePointIds">知识点id数组</param>
    /// <returns></returns>
    Task<List<BrowseCoursePracticeViewModel>> GetPracticeInfoByKnowledgePointId(Guid courseId, List<Guid> knowledgePointIds);

    /// <summary>
    /// 获取指定课程的考试频度对应的数量
    /// </summary>
    /// <param name="subjectId"></param>
    /// <returns></returns>
    Task<ExamFrequencyQuestionCountViewModel> GetExamFrequencyQuestionCount(Guid subjectId);
}