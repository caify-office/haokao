using Girvs.Refit;
using HaoKao.Common.RemoteModel;
using HaoKao.Common.RemoteService;
using Microsoft.AspNetCore.Mvc;
using Refit;
using System.Threading.Tasks;

namespace HaoKao.LearningPlanService.Infrastructure.RemoteService;

[RefitService(RefitServiceNames.CourseService)]
public interface IRemoteCourseService : IGirvsRefit
{
    [Get($"{URLPrefixManager.CourseVideoWeChatPrefix}/VideoIdsByCourseIds")]
    Task<dynamic> GetVideoIdsByCourseIds([FromQuery] string courseIds);


    /// <summary>
    /// 根据查询获取列表，用于分页(读取课程下面所有的课程视频id)
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    [Get($"{URLPrefixManager.CourseVideoWeChatPrefix}/CourseVideoList")]
    Task<dynamic> GetCourseVideoList([FromQuery] QueryCourseVideoInfo queryViewModel);


    /// <summary>
    /// 根据课程章节id获取指定
    /// </summary>
    /// <param name="courseChapterId">课程章节id</param>
    [Get($"{URLPrefixManager.CoursePracticeWechatPrefix}/{{courseChapterId}}")]
    Task<dynamic> GetCoursePractice([FromHeader]Guid courseChapterId);

    /// <summary>
    /// 根据课程id和章节id获取指定章节练习(智辅课程专用)
    /// </summary>
    /// <param name="courseId">课程id</param>
    /// <param name="courseChapterId">Id</param>
    [Get($"{URLPrefixManager.CoursePracticeWechatPrefix}/ChapterPractice/{{courseId}}/{{courseChapterId}}")]
    Task<dynamic> GetChapterPractice([FromHeader] Guid courseId, [FromHeader]Guid courseChapterId);



    /// <summary>
    /// 按照课程id获取所有课程章节
    /// </summary>
    /// <param name="courseId">课程id</param>
    /// <returns></returns>
    [Get("/api/WeChat/CourseChapterWeChatService/All/{courseId}")]
    Task<dynamic> GetAllCourseChapterAsync(Guid courseId);
   
}


