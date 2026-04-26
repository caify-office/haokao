using HaoKao.AgreementService.Application.ViewModels.CourseAgreement;

namespace HaoKao.AgreementService.Application.Services.Web;

public interface ICourseAgreementWebService : IAppWebApiService, IManager
{
    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    Task<BrowseCourseAgreementViewModel> Get(Guid id);

    /// <summary>
    /// 根据ids获取列表
    /// </summary>
    /// <param name="ids">查询对象</param>
    Task<List<BrowseCourseAgreementViewModel>> Get(IReadOnlyList<Guid> ids);

    /// <summary>
    /// 获取公共协议Id和名称
    /// </summary>
    /// <param name="id">主键</param>
    /// <returns></returns>
    Task<Dictionary<Guid, string>> GetWithCommonAgreement(Guid? id);
}