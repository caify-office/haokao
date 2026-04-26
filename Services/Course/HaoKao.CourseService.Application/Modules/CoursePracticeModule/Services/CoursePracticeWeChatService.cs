using HaoKao.CourseService.Application.Modules.CoursePracticeModule.Interfaces;
using HaoKao.CourseService.Application.Modules.CoursePracticeModule.ViewModels;

namespace HaoKao.CourseService.Application.Modules.CoursePracticeModule.Services;

/// <summary>
/// 课后练习接口服务-小程序端
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.WeChat)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class CoursePracticeWeChatService(ICoursePracticeWebService service) : ICoursePracticeWeChatService
{
    /// <summary>
    /// 根据课程章节id获取指定
    /// </summary>
    /// <param name="courseChapterId">课程章节id</param>
    [HttpGet("{courseChapterId}")]
    public Task<BrowseCoursePracticeViewModel> Get(Guid courseChapterId)
    {
        return service.Get(courseChapterId);
    }

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    [HttpGet]
    public Task<QueryCoursePracticeViewModel> Get([FromQuery] QueryCoursePracticeViewModel queryViewModel)
    {
        return service.Get(queryViewModel);
    }

    /// <summary>
    /// 根据课程id和章节id获取指定章节练习(智辅课程专用)
    /// </summary>
    /// <param name="courseId">课程id</param>
    /// <param name="courseChapterId">Id</param>
    [HttpGet("{courseId}/{courseChapterId}")]
    public Task<BrowseCoursePracticeViewModel> GetChapterPractice(Guid courseId, Guid courseChapterId)
    {
        return service.GetChapterPractice(courseId, courseChapterId);
    }

    /// <summary>
    /// 通过知识点Id获取关联知识点对应的课后练习
    /// </summary>
    /// <param name="courseId">课程id</param>
    /// <param name="knowledgePointIds">知识点id数组</param>
    /// <returns></returns>
    [HttpPost]
    public Task<List<BrowseCoursePracticeViewModel>> GetPracticeInfoByKnowledgePointId(Guid courseId, [FromBody] List<Guid> knowledgePointIds)
    {
        return service.GetPracticeInfoByKnowledgePointId(courseId, knowledgePointIds);
    }

    /// <summary>
    /// 获取指定课程的考试频度对应的数量
    /// </summary>
    /// <param name="subjectId"></param>
    /// <returns></returns>
    [HttpGet]
    public Task<ExamFrequencyQuestionCountViewModel> GetExamFrequencyQuestionCount(Guid subjectId)
    {
        return service.GetExamFrequencyQuestionCount(subjectId);
    }
}