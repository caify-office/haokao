using HaoKao.KnowledgePointService.Application.ViewModels.KnowledgePoint;

namespace HaoKao.KnowledgePointService.Application.Services.Management;

public interface IKnowledgePointService : IAppWebApiService, IManager
{
    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    Task<KnowledgePointBrowseViewModel> Get(Guid id);

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    Task<KnowledgePointQueryViewModel> Get(KnowledgePointQueryViewModel queryViewModel);

    /// <summary>
    /// 创建注册用户
    /// </summary>
    /// <param name="model">新增模型</param>
    Task Create(KnowledgePointCreateViewModel model);

    /// <summary>
    /// 根据主键删除指定知识点
    /// </summary>
    /// <param name="id">主键</param>
    Task Delete(Guid id);

    /// <summary>
    /// 根据主键更新指定知识点
    /// </summary>
    /// <param name="id">主键</param>
    /// <param name="model">新增模型</param>
    Task Update(Guid id, KnowledgePointUpdateViewModel model);
}