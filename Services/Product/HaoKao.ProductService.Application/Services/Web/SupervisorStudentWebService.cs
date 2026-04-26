using HaoKao.ProductService.Application.Services.Management;
using HaoKao.ProductService.Application.ViewModels.SupervisorStudent;

namespace HaoKao.ProductService.Application.Services.Web;

/// <summary>
/// 督学学员接口服务-web端
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.WebSite)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]

public class SupervisorStudentWebService : ISupervisorStudentWebService
{
    #region 初始参数
    private readonly ISupervisorStudentService _studentService;

    #endregion

    #region 构造函数

    /// <summary>
    /// 构造函数
    /// </summary>
    public SupervisorStudentWebService(ISupervisorStudentService studentService)
    {
        _studentService = studentService ?? throw new ArgumentNullException(nameof(studentService));
    }

    #endregion

    #region 服务方法
    /// <summary>
    /// 创建督学学员
    /// </summary>
    /// <param name="model">新增模型</param>
    [HttpPost]
    public  Task Create([FromBody] CreateSupervisorStudentViewModel model)
    {
        return _studentService.Create(model);
    }
    #endregion
}