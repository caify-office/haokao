using HaoKao.Common.Enums;
using HaoKao.CourseService.Application.Modules.CoursePracticeModule.Interfaces;
using HaoKao.CourseService.Application.Modules.CoursePracticeModule.ViewModels;
using HaoKao.CourseService.Domain.CoursePracticeModule;

namespace HaoKao.CourseService.Application.Modules.CoursePracticeModule.Services;

/// <summary>
/// 课后练习接口服务-web端
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.WebSite)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class CoursePracticeWebService(
    ICoursePracticeService service,
    ICoursePracticeRepository repository,
    IStaticCacheManager cacheManager
) : ICoursePracticeWebService
{
    /// <summary>
    /// 根据课程章节id获取指定
    /// </summary>
    /// <param name="courseChapterId">课程章节id</param>
    [HttpGet("{courseChapterId:guid}")]
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
    [HttpGet("{courseId:guid}/{courseChapterId:guid}")]
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
    public async Task<List<BrowseCoursePracticeViewModel>> GetPracticeInfoByKnowledgePointId(Guid courseId, [FromBody] List<Guid> knowledgePointIds)
    {
        var list = await repository.GetWhereAsync(x => x.CourseId == courseId && knowledgePointIds.Contains(x.KnowledgePointId));
        return list.MapTo<List<BrowseCoursePracticeViewModel>>();
    }

    /// <summary>
    /// 获取指定课程的考试频度对应的数量
    /// </summary>
    /// <param name="subjectId"></param>
    /// <returns></returns>
    [HttpGet]
    public Task<ExamFrequencyQuestionCountViewModel> GetExamFrequencyQuestionCount(Guid subjectId)
    {
        var key = $"{nameof(GetExamFrequencyQuestionCount)}:subjectId_{subjectId}";
        var cacheKey = GirvsEntityCacheDefaults<CoursePractice>.QueryCacheKey.Create(key);
        return cacheManager.GetAsync(cacheKey, async () =>
        {
            var list = await repository.GetWhereAsync(x => x.SubjectId == subjectId);
            return new ExamFrequencyQuestionCountViewModel
            {
                High = list.Where(x => x.ExamFrequency == ExamFrequency.High).Sum(x => x.QuestionCount),
                Medium = list.Where(x => x.ExamFrequency == ExamFrequency.Medium).Sum(x => x.QuestionCount),
                Low = list.Where(x => x.ExamFrequency == ExamFrequency.Low).Sum(x => x.QuestionCount)
            };
        });
    }
}