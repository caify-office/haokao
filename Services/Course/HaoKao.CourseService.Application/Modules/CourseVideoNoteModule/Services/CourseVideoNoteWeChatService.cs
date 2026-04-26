using HaoKao.CourseService.Application.Modules.CourseVideoNoteModule.Interfaces;
using HaoKao.CourseService.Application.Modules.CourseVideoNoteModule.ViewModels;

namespace HaoKao.CourseService.Application.Modules.CourseVideoNoteModule.Services;

/// <summary>
/// 课程视频笔记接口服务-小程序端
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.WeChat)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class CourseVideoNoteWeChatService(ICourseVideoNoteWebService service) : ICourseVideoNoteWeChatService
{
    #region 初始参数

    private readonly ICourseVideoNoteWebService _service = service ?? throw new ArgumentNullException(nameof(service));

    #endregion

    #region 服务方法

    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    [HttpGet("{id}")]
    public Task<BrowseCourseVideoNoteViewModel> Get(Guid id)
    {
        return _service.Get(id);
    }

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    [HttpGet]
    public Task<QueryCourseVideoNoteViewModel> Get([FromQuery] QueryCourseVideoNoteViewModel queryViewModel)
    {
        return _service.Get(queryViewModel);
    }

    /// <summary>
    /// 创建课程视频笔记
    /// </summary>
    /// <param name="model">新增模型</param>
    [HttpPost]
    public Task Create([FromBody] CreateCourseVideoNoteViewModel model)
    {
        return _service.Create(model);
    }

    /// <summary>
    /// 根据主键删除指定课程视频笔记
    /// </summary>
    /// <param name="id">主键</param>
    [HttpDelete("{id}")]
    public Task Delete(Guid id)
    {
        return _service.Delete(id);
    }

    /// <summary>
    /// 根据主键更新指定课程视频笔记
    /// </summary>
    /// <param name="model">新增模型</param>
    [HttpPut]
    public Task Update([FromBody] UpdateCourseVideoNoteViewModel model)
    {
        return _service.Update(model);
    }

    #endregion
}