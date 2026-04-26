using HaoKao.CourseService.Application.Modules.CourseVideoNoteModule.ViewModels;

namespace HaoKao.CourseService.Application.Modules.CourseVideoNoteModule.Interfaces;

public interface ICourseVideoNoteWeChatService : IAppWebApiService, IManager
{
    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    Task<BrowseCourseVideoNoteViewModel> Get(Guid id);

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    Task<QueryCourseVideoNoteViewModel> Get(QueryCourseVideoNoteViewModel queryViewModel);

    /// <summary>
    /// 创建课程视频笔记
    /// </summary>
    /// <param name="model">新增模型</param>
    Task Create(CreateCourseVideoNoteViewModel model);

    /// <summary>
    /// 根据主键删除指定课程视频笔记
    /// </summary>
    /// <param name="id">主键</param>
    Task Delete(Guid id);

    /// <summary>
    /// 根据主键更新指定课程视频笔记
    /// </summary>
    /// <param name="model">新增模型</param>
    Task Update(UpdateCourseVideoNoteViewModel model);
}