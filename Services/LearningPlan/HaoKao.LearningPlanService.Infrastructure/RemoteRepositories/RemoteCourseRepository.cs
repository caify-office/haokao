using HaoKao.Common.RemoteModel;
using HaoKao.LearningPlanService.Domain.RemoteRepositories;
using HaoKao.LearningPlanService.Infrastructure.RemoteService;
using Refit;
using System.Threading.Tasks;

namespace HaoKao.LearningPlanService.Infrastructure.RemoteRepositories;

public class RemoteCourseRepository : IRemoteCourseRepository
{
    /// <summary>
    /// 测试根据课程ids拿到多个课程下面所有的课程视频信息
    /// </summary>
    /// <param name="courseIds"></param>
    /// <returns></returns>
    public async Task<List<CourseVideoQueryListInfo>> GetVideoIdsByCourseIds(string courseIds)
    {
        var remote = EngineContext.Current.Resolve<IRemoteCourseService>();
        var paper = await remote.GetVideoIdsByCourseIds(courseIds);
        var result = JsonConvert.DeserializeObject<List<CourseVideoQueryListInfo>>(paper.GetRawText());
        return result;
    }

    /// <summary>
    /// 测试根据课程ids拿到多个课程下面所有的课程视频信息
    /// </summary>
    /// <param name="queryViewModel"></param>
    /// <returns></returns>
    public async Task<QueryCourseVideoInfo> GetCourseVideoList(QueryCourseVideoInfo queryViewModel)
    {
        queryViewModel.QueryFields = null;
        var remote = EngineContext.Current.Resolve<IRemoteCourseService>();
        var response = await remote.GetCourseVideoList(queryViewModel);
        var result = JsonConvert.DeserializeObject<QueryCourseVideoInfo>(response.GetRawText());
        return result;
    }

    /// <summary>
    /// 根据课程章节id获取指定
    /// </summary>
    /// <param name="courseChapterId">课程章节id</param>
    public async Task<BrowseCoursePracticeInfo> GetCoursePractice(Guid courseChapterId)
    {
        var remote = EngineContext.Current.Resolve<IRemoteCourseService>();
        dynamic response;
        try
        {
            response = await remote.GetCoursePractice(courseChapterId);
        }
        catch (ApiException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NoContent)
        {
            return null;
        }


        var result = JsonConvert.DeserializeObject<BrowseCoursePracticeInfo>(response.GetRawText());
        return result;
    }

    /// <summary>
    /// 根据课程id和章节id获取指定章节练习(智辅课程专用)
    /// </summary>
    /// <param name="courseId">课程id</param>
    /// <param name="courseChapterId">Id</param>
    public async Task<BrowseCoursePracticeInfo> GetChapterPractice(Guid courseId, Guid courseChapterId)
    {
        var remote = EngineContext.Current.Resolve<IRemoteCourseService>();
        dynamic response;
        try
        {
            response = await remote.GetChapterPractice(courseId, courseChapterId);
        }
        catch (ApiException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NoContent)
        {
            return null;
        }
        var result = JsonConvert.DeserializeObject<BrowseCoursePracticeInfo>(response.GetRawText());
        return result;
    }

    public async Task<List<BrowseCourseChapterViewModel>> GetAllCourseChapterAsync(Guid courseId)
    {
        var remote = EngineContext.Current.Resolve<IRemoteCourseService>();
        var response = await remote.GetAllCourseChapterAsync(courseId);
        var result = JsonConvert.DeserializeObject<List<BrowseCourseChapterViewModel>>(response.GetRawText());
        return result;
    }
}