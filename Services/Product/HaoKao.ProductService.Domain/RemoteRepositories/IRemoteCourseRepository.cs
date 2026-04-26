using Girvs.BusinessBasis;

namespace HaoKao.ProductService.Domain.RemoteRepositories
{
    public interface IRemoteCourseRepository:IManager
    {
        /// <summary>
        /// 测试根据课程ids拿到多个课程下面所有的课程视频信息
        /// </summary>
        /// <param name="courseIds"></param>
        /// <returns></returns>
        Task<List<CourseVideoQueryListInfo>> GetVideoIdsByCourseIds(string courseIds);

   

        /// <summary>
        /// 根据课程章节id获取指定
        /// </summary>
        /// <param name="courseChapterId">课程章节id</param>
        Task<BrowseCoursePracticeInfo> GetCoursePractice(Guid courseChapterId);




        /// <summary>
        /// 按照课程id获取所有课程章节
        /// </summary>
        /// <param name="courseId">课程id</param>
        /// <returns></returns>
        Task<List<BrowseCourseChapterViewModel>> GetAllCourseChapterAsync(Guid courseId);
    }
}
