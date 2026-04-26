using HaoKao.CourseService.Application.Modules.CourseChapterModule.ViewModels;

namespace HaoKao.CourseService.Application.Modules.CourseChapterModule.Interfaces;

public interface ICourseChapterService : IAppWebApiService, IManager
{
    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    Task<BrowseCourseChapterViewModel> Get(Guid id);

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    Task<QueryCourseChapterViewModel> Get(QueryCourseChapterViewModel queryViewModel);

    /// <summary>
    /// 获取数据-章节树列表
    /// </summary>
    /// <param name="id">展开的节点</param>
    /// <param name="courseId">课程id</param>
    /// <returns></returns>
    Task<List<dynamic>> GetTreeByQueryAsync(Guid? id, Guid? courseId);

    // <summary>
    /// 按照课程id获取所有课程章节
    /// </summary>
    /// <param name="courseId">课程id</param>
    /// <returns></returns>
    Task<List<BrowseCourseChapterViewModel>> GetAllAsync(Guid courseId);

    /// <summary>
    /// 创建课程章节
    /// </summary>
    /// <param name="model">新增模型</param>
    Task Create(CreateCourseChapterViewModel model);

    /// <summary>
    /// 批量创建课程章节
    /// </summary>
    /// <param name="models"></param>
    /// <returns></returns>
    Task BatchCreate(List<CreateCourseChapterViewModel> models);

    /// <summary>
    /// 根据主键删除指定课程章节
    /// </summary>
    /// <param name="id">主键</param>
    Task Delete(Guid id);

    /// <summary>
    /// 清空目录
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task BatchDelete(Guid id);

    /// <summary>
    /// 根据主键更新指定课程章节
    /// </summary>
    /// <param name="id">主键</param>
    /// <param name="model">新增模型</param>
    Task Update(Guid id, UpdateCourseChapterViewModel model);
}