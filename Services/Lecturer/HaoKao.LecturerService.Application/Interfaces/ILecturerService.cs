using HaoKao.LecturerService.Application.ViewModels;
using System.Collections.Generic;

namespace HaoKao.LecturerService.Application.Interfaces;

public interface ILecturerService : IAppWebApiService, IManager
{
    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    Task<BrowseLecturerViewModel> Get(Guid id);

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    Task<LecturerQueryViewModel> Get(LecturerQueryViewModel queryViewModel);

    /// <summary>
    /// 根据主键数组获取讲师列表
    /// </summary>
    /// <param name="ids">主键</param>
    Task<IReadOnlyList<BrowseLecturerViewModel>> GetByIds(Guid[] ids);

    /// <summary>
    /// 创建讲师
    /// </summary>
    /// <param name="model">新增模型</param>
    Task Create(CreateLecturerViewModel model);

    /// <summary>
    /// 根据主键删除指定讲师
    /// </summary>
    /// <param name="id">主键</param>
    Task Delete(Guid id);

    /// <summary>
    /// 根据主键更新指定讲师
    /// </summary>
    /// <param name="model">新增模型</param>
    Task Update(UpdateLecturerViewModel model);
}