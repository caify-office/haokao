using Girvs.Refit;
using HaoKao.Common.RemoteService;
using Microsoft.AspNetCore.Mvc;
using Refit;

namespace HaoKao.ProductService.Infrastructure.RemoteService;

[RefitService(RefitServiceNames.CourseService)]
public interface IRemoteCourseService : IGirvsRefit
{
    [Get($"{URLPrefixManager.CourseVideoManagementPrefix}/VideoIdsByCourseIds")]
    Task<dynamic> GetVideoIdsByCourseIds([FromQuery] string courseIds);

    /// <summary>
    /// 根据课程章节id获取指定
    /// </summary>
    /// <param name="courseChapterId">课程章节id</param>
    [Get($"{URLPrefixManager.CoursePracticeManagementPrefix}/{{courseChapterId}}")]
    Task<dynamic> GetCoursePractice([FromHeader] Guid courseChapterId);

    /// <summary>
    /// 按照课程id获取所有课程章节
    /// </summary>
    /// <param name="courseId">课程id</param>
    /// <returns></returns>
    [Get($"{URLPrefixManager.CourseChaperManagementPrefix}/All/{{courseId}}")]
    Task<dynamic> GetAllCourseChapterAsync(Guid courseId);
}