using HaoKao.ProductService.Application.Services.Web;
using HaoKao.ProductService.Application.ViewModels.SupervisorStudent;

namespace HaoKao.ProductService.Application.Services.WeChat;

/// <summary>
/// 督学学员接口服务-微信小程序端
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.WeChat)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]

public class SupervisorStudentWeChatService : ISupervisorStudentWeChatService
{
    #region 初始参数
    private readonly ISupervisorStudentWebService _studentWebService;

    #endregion

    #region 构造函数

    /// <summary>
    /// 构造函数
    /// </summary>
    public SupervisorStudentWeChatService(ISupervisorStudentWebService studentService)
    {
        _studentWebService = studentService ?? throw new ArgumentNullException(nameof(studentService));
    }

    #endregion

    #region 服务方法
    /// <summary>
    /// 创建督学学员
    /// </summary>
    /// <param name="model">新增模型</param>
    [HttpPost]
    public Task Create([FromBody] CreateSupervisorStudentViewModel model)
    {
        return _studentWebService.Create(model);
    }
    #endregion
}