using HaoKao.PaperTempleteService.Application.ViewModels;
using HaoKao.PaperTempleteService.Domain.Entities;
using System.Collections.Generic;

namespace HaoKao.PaperTempleteService.Application.Services;

public interface IPaperTempleteService : IAppWebApiService, IManager
{
    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    Task<BrowsePaperTempleteViewModel> Get(Guid id);

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    Task<PaperTempleteQueryViewModel> Get(PaperTempleteQueryViewModel queryViewModel);

    /// <summary>
    /// 根据查询获取列表
    /// </summary>
    /// <param name="subjectId">科目Id</param>
    Task<IReadOnlyList<PaperTemplete>> GetListBySubjectId(string subjectId);

    /// <summary>
    /// 创建
    /// </summary>
    /// <param name="model">新增模型</param>
    Task Create(CreatePaperTempleteViewModel model);

    /// <summary>
    /// 根据主键删除指定
    /// </summary>
    /// <param name="id">主键</param>
    Task Delete(Guid id);

    /// <summary>
    /// 根据主键更新指定
    /// </summary>
    /// <param name="id">主键</param>
    /// <param name="model">新增模型</param>
    Task Update(Guid id, UpdatePaperTempleteViewModel model);
}