using HaoKao.CourseService.Application.Modules.CourseMaterialsModule.ViewModels;
using HaoKao.CourseService.Domain.CourseMaterialsModule;

namespace HaoKao.CourseService.Application.Modules.CourseMaterialsModule.Interfaces;

public interface ICourseMaterialsWebService : IAppWebApiService, IManager
{
    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    Task<QueryCourseMaterialsViewModel> Get(QueryCourseMaterialsViewModel queryViewModel);

    /// <summary>
    /// 下载讲义（阶段学习传课程章节Id，智辅课程学习传课程Id）
    /// </summary>
    /// <param name="CourseId">（阶段学习传课程章节Id，智辅课程学习传课程Id）</param>
    /// <returns></returns>
    Task<List<CourseMaterials>> DownLoadMaterials(Guid CourseId);
}