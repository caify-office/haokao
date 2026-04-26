using HaoKao.PaperService.Application.ViewModels;

namespace HaoKao.PaperService.Application.Interfaces;

public interface IPaperService : IAppWebApiService, IManager
{
    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    Task<BrowsePaperViewModel> Get(Guid id);

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    Task<PaperQueryViewModel> Get(PaperQueryViewModel queryViewModel);

    /// <summary>
    /// 根据主键获取基本信息以及题型介绍
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<BrowsePaperViewModel> GetPaperDetailInfo(Guid id);

    /// <summary>
    /// 创建试卷
    /// </summary>
    /// <param name="model">新增模型</param>
    Task Create(CreatePaperViewModel model);

    /// <summary>
    /// 根据主键删除指定试卷
    /// </summary>
    /// <param name="id">主键</param>
    Task Delete(Guid id);

    /// <summary>
    /// 根据主键更新指定试卷
    /// </summary>
    /// <param name="model">新增模型</param>
    Task Update(UpdatePaperViewModel model);
}