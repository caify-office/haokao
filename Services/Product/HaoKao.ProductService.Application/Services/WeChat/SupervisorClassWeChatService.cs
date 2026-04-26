using Girvs.Driven.Extensions;
using HaoKao.ProductService.Application.Services.Management;
using HaoKao.ProductService.Application.Services.Web;
using HaoKao.ProductService.Application.ViewModels.SupervisorClass;
using HaoKao.ProductService.Domain.Commands.SupervisorClass;
using HaoKao.ProductService.Domain.Entities;
using HaoKao.ProductService.Domain.Queries;
using HaoKao.ProductService.Domain.Repositories;

namespace HaoKao.ProductService.Application.Services.WeChat;

/// <summary>
/// 班级督学接口服务-微信小程序端接口
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.WeChat)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class SupervisorClassWeChatService : ISupervisorClassWeChatService
{
    #region 初始参数

    private readonly ISupervisorClassWebService _service;

    #endregion

    #region 构造函数

    /// <summary>
    /// 构造函数
    /// </summary>
    public SupervisorClassWeChatService(ISupervisorClassWebService service)
    {

        _service = service ?? throw new ArgumentNullException(nameof(service));
    }

    #endregion

    #region 服务方法

    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    [HttpGet("{id}")]
    public Task<BrowseSupervisorClassViewModel> Get(Guid id)
    {
        return _service.Get(id);
    }

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    [HttpGet]

    public Task<SupervisorClassQueryViewModel> Get([FromQuery] SupervisorClassQueryViewModel queryViewModel)
    {
        return _service.Get(queryViewModel);
    }
    #endregion
}