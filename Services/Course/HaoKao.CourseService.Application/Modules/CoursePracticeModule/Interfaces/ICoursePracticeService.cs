using HaoKao.CourseService.Application.Modules.CoursePracticeModule.ViewModels;

namespace HaoKao.CourseService.Application.Modules.CoursePracticeModule.Interfaces;

public interface ICoursePracticeService : IAppWebApiService, IManager
{
    /// <summary>
    /// 根据课程章节id获取指定
    /// </summary>
    /// <param name="courseChapterId">课程章节id</param>
    Task<BrowseCoursePracticeViewModel> Get(Guid courseChapterId);

    /// <summary>
    /// 根据课程id和章节id获取指定章节练习(智辅课程专用)
    /// </summary>
    /// <param name="courseId">课程id</param>
    /// <param name="courseChapterId">Id</param>
    Task<BrowseCoursePracticeViewModel> GetChapterPractice(Guid courseId, Guid courseChapterId);

    /// <summary>
    /// 列表查询
    /// </summary>
    /// <param name="queryViewModel"></param>
    /// <returns></returns>
    Task<QueryCoursePracticeViewModel> Get(QueryCoursePracticeViewModel queryViewModel);

    /// <summary>
    /// 根据主键删除指定课后练习
    /// </summary>
    /// <param name="id">主键</param>
    Task Delete(Guid id);

    /// <summary>
    /// 根据主键更新指定课后练习
    /// </summary>
    /// <param name="model">新增模型</param>
    Task Save(SaveCoursePracticeViewModel model);

    /// <summary>
    /// 保存智辅课程练习（智辅课程专用）
    /// </summary>
    /// <param name="model">新增模型</param>
    Task SaveAssistantCoursePractice([FromBody] SaveAssistantCoursePracticeViewModel model);
}