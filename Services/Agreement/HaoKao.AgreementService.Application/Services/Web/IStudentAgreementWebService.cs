using HaoKao.AgreementService.Application.ViewModels.StudentAgreement;

namespace HaoKao.AgreementService.Application.Services.Web;

public interface IStudentAgreementWebService : IAppWebApiService, IManager
{
    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    Task<SignedStudentAgreementViewModel> Get(Guid id);

    /// <summary>
    /// 获取用户已签署的记录
    /// </summary>
    /// <param name="queryViewModel"></param>
    /// <returns></returns>
    Task<List<MyAgreementListViewModel>> GetMyAgreement(List<QueryMyAgreementViewModel> queryViewModel);

    /// <summary>
    /// 产品是否签署协议
    /// </summary>
    /// <param name="productId"></param>
    /// <returns></returns>
    Task<bool> HasBeenSigned(Guid productId);

    /// <summary>
    /// 创建学员协议
    /// </summary>
    /// <param name="model">新增模型</param>
    Task<Guid> Create(CreateStudentAgreementViewModel model);
}