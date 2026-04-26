using HaoKao.StudentService.Application.Interfaces;
using HaoKao.StudentService.Application.ViewModels;

namespace HaoKao.StudentService.Application.Services;

/// <summary>
/// 学员接口服务--WeChat端
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.WeChat)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class StudentWeChatService(IStudentWebService service) : IStudentWeChatService
{
    /// <summary>
    /// 获取当前租户下的学员信息
    /// </summary>
    [HttpGet]
    public Task<BrowseStudentViewModel> Get()
    {
        return service.Get();
    }
}