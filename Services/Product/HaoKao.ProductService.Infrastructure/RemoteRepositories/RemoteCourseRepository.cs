using Girvs.Infrastructure;
using HaoKao.Common.RemoteModel;
using HaoKao.ProductService.Domain.RemoteRepositories;
using HaoKao.ProductService.Infrastructure.RemoteService;
using Refit;

namespace HaoKao.ProductService.Infrastructure.RemoteRepositories
{
    public class RemoteCourseRepository : IRemoteCourseRepository
    {
        /// <summary>
        /// 测试根据课程ids拿到多个课程下面所有的课程视频信息
        /// </summary>
        /// <param name="courseIds"></param>
        /// <returns></returns>
        public async Task<List<CourseVideoQueryListInfo>> GetVideoIdsByCourseIds(string courseIds)
        {
            var remot = EngineContext.Current.Resolve<IRemoteCourseService>();
            var paper = await remot.GetVideoIdsByCourseIds(courseIds);
            var result = JsonConvert.DeserializeObject<List<CourseVideoQueryListInfo>>(paper.GetRawText());
            return result;
        }

        /// <summary>
        /// 根据课程章节id获取指定
        /// </summary>
        /// <param name="courseChapterId">课程章节id</param>
        public async Task<BrowseCoursePracticeInfo> GetCoursePractice(Guid courseChapterId)
        {
            var remot = EngineContext.Current.Resolve<IRemoteCourseService>();
            dynamic reponse;
            try
            {
                reponse = await remot.GetCoursePractice(courseChapterId);
            }
            catch (ApiException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return null;
            }


            var result = JsonConvert.DeserializeObject<BrowseCoursePracticeInfo>(reponse.GetRawText());
            return result;
        }

        public async Task<List<BrowseCourseChapterViewModel>> GetAllCourseChapterAsync(Guid courseId)
        {
            var remot = EngineContext.Current.Resolve<IRemoteCourseService>();
            var reponse = await remot.GetAllCourseChapterAsync(courseId);
            var result = JsonConvert.DeserializeObject<List<BrowseCourseChapterViewModel>>(reponse.GetRawText());
            return result;
        }
    }
}