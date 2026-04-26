using HaoKao.StudyMaterialService.Application.ViewModels;

namespace HaoKao.StudyMaterialService.Application.Interfaces;

public interface IStudyMaterialService : IAppWebApiService, IManager
{
    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    Task<BrowseStudyMaterialViewModel> Get(Guid id);

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    Task<QueryStudyMaterialViewModel> Get(QueryStudyMaterialViewModel queryViewModel);

    /// <summary>
    /// 创建学习资料
    /// </summary>
    /// <param name="model">新增模型</param>
    Task Create(CreateStudyMaterialViewModel model);

    /// <summary>
    /// 批量删除指定学习资料
    /// </summary>
    /// <param name="ids">ids</param>
    Task Delete(List<Guid> ids);

    /// <summary>
    /// 根据主键更新指定学习资料
    /// </summary>
    /// <param name="model">更新模型</param>
    Task Update(UpdateStudyMaterialViewModel model);

    /// <summary>
    /// 批量启用/禁用学习资料
    /// </summary>
    /// <param name="model">启用/禁用模型</param>
    Task Enable(EnableStudyMaterialViewModel model);
}