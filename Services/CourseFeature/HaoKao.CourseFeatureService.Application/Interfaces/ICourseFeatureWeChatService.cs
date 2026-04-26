using HaoKao.CourseFeatureService.Application.ViewModels;

namespace HaoKao.CourseFeatureService.Application.Interfaces;

public interface ICourseFeatureWeChatService : IAppWebApiService, IManager
{
    /// <summary>
    /// 根据ids获取列表
    /// </summary>
    /// <param name="ids"></param>
    Task<List<BrowseCourseFeatureWebViewModel>> Get(List<Guid> ids);
}