using HaoKao.AgreementService.Application.Services.Web;
using HaoKao.AgreementService.Application.ViewModels.StudentAgreement;

namespace HaoKao.AgreementService.Application.Services.WeChat;

/// <summary>
/// 学员协议接口服务
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.WeChat)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class StudentAgreementWeChatService(IStudentAgreementWebService service) : IStudentAgreementWeChatService
{
    private readonly IStudentAgreementWebService _service = service ?? throw new ArgumentNullException(nameof(service));

    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    [HttpGet("{id:guid}")]
    public Task<SignedStudentAgreementViewModel> Get(Guid id)
    {
        return _service.Get(id);
    }

    /// <summary>
    /// 获取用户已签署的记录(我的协议)
    /// </summary>
    /// <param name="queryViewModel"></param>
    /// <returns></returns>
    [HttpPost]
    public Task<List<MyAgreementListViewModel>> GetMyAgreement([FromBody] List<QueryMyAgreementViewModel> queryViewModel)
    {
        return _service.GetMyAgreement(queryViewModel);
    }

    /// <summary>
    /// 产品是否签署协议
    /// </summary>
    /// <param name="productId"></param>
    /// <returns></returns>
    [HttpGet("{productId:guid}")]
    public Task<bool> HasBeenSigned(Guid productId)
    {
        return _service.HasBeenSigned(productId);
    }

    /// <summary>
    /// 创建学员协议
    /// </summary>
    /// <param name="model">新增模型</param>
    [HttpPost]
    public Task<Guid> Create([FromBody] CreateStudentAgreementViewModel model)
    {
        return _service.Create(model);
    }
}