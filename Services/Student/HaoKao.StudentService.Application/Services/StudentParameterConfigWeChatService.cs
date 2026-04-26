using HaoKao.StudentService.Application.Interfaces;
using HaoKao.StudentService.Application.ViewModels;

namespace HaoKao.StudentService.Application.Services;

/// <summary>
/// 学员参数设置接口服务-WeChat端
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.WeChat)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class StudentParameterConfigWeChatService(IStudentParameterConfigWebService service) : IStudentParameterConfigWeChatService
{
    private readonly IStudentParameterConfigWebService _service = service ?? throw new ArgumentNullException(nameof(service));

    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    [HttpGet("{id}")]
    public Task<BrowseStudentParameterConfigViewModel> Get(Guid id)
    {
        return _service.Get(id);
    }

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="model">查询对象</param>
    [HttpGet]
    public Task<QueryStudentParameterConfigViewModel> Get([FromQuery] QueryStudentParameterConfigViewModel model)
    {
        return _service.Get(model);
    }

    /// <summary>
    /// 保存学员参数设置
    /// </summary>
    /// <param name="model">新增模型</param>
    [HttpPost]
    public Task Save([FromBody] CreateStudentParameterConfigViewModel model)
    {
        return _service.Save(model);
    }

    /// <summary>
    /// 条件查询(隔日生效)
    /// </summary>
    /// <param name="model">查询对象</param>
    [HttpGet]
    public Task<BrowseStudentParameterConfigViewModel> GetEffectiveNextDay([FromQuery] StudentParameterConfigQueryEffectiveNextDayViewModel model)
    {
        return _service.GetEffectiveNextDay(model);
    }

    /// <summary>
    /// 保存学员参数设置(隔日生效)
    /// </summary>
    /// <param name="model">新增模型</param>
    [HttpPost]
    public Task SaveEffectiveNextDay([FromBody] CreateStudentParameterConfigViewModel model)
    {
        return _service.SaveEffectiveNextDay(model);
    }
}