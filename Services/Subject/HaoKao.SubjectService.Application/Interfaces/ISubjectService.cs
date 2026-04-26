using HaoKao.SubjectService.Application.ViewModels;

namespace HaoKao.SubjectService.Application.Interfaces;

public interface ISubjectService : IAppWebApiService, IManager
{
    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    Task<BrowseSubjectViewModel> Get(Guid id);

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    Task<SubjectQueryViewModel> Get(SubjectQueryViewModel queryViewModel);

    /// <summary>
    /// 读取科目列表,不带分页
    /// </summary>
    Task<IReadOnlyList<BrowseSubjectViewModel>> GetSubjectList();

    /// <summary>
    /// 创建科目
    /// </summary>
    /// <param name="model">新增模型</param>
    Task Create(CreateSubjectViewModel model);

    /// <summary>
    /// 根据主键删除指定科目
    /// </summary>
    /// <param name="id">主键</param>
    Task Delete(Guid id);

    /// <summary>
    /// 根据主键更新指定科目
    /// </summary>
    /// <param name="id">主键</param>
    /// <param name="model">新增模型</param>
    Task Update(Guid id, UpdateSubjectViewModel model);
}