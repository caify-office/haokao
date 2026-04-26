using HaoKao.SubjectService.Application.ViewModels;

namespace HaoKao.SubjectService.Application.Interfaces;

public interface ISubjectWeChatService : IAppWebApiService, IManager
{
    /// <summary>
    /// 根据查询获取列表，不分页
    /// </summary>
    Task<IReadOnlyList<BrowseSubjectViewModel>> Get();

    /// <summary>
    /// 按租户Id获取科目列表
    /// </summary>
    Task<IReadOnlyList<BrowseSubjectViewModel>> GetListByTenantId(Guid tenantId);

    /// <summary>
    /// 获取公共科目列表
    /// </summary>
    Task<IReadOnlyList<BrowseSubjectViewModel>> GetCommonSubjectList();
}