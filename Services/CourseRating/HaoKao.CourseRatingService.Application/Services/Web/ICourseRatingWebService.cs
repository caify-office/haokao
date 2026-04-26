using HaoKao.CourseRatingService.Application.ViewModels;

namespace HaoKao.CourseRatingService.Application.Services.Web;

public interface ICourseRatingWebService : IAppWebApiService, IManager
{
    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    Task<QueryCourseRatingWebViewModel> Get(QueryCourseRatingWebViewModel queryViewModel);

    /// <summary>
    /// 创建课程评价
    /// </summary>
    /// <param name="model">新增模型</param>
    Task Create(CreateCourseRatingViewModel model);
}