namespace HaoKao.LiveBroadcastService.Application.Services.Management;

public interface ISensitiveWordService : IAppWebApiService, IManager
{
    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    Task<SensitiveWordQueryViewModel> Get(SensitiveWordQueryViewModel queryViewModel);

    /// <summary>
    /// 创建敏感词
    /// </summary>
    /// <param name="model">新增模型</param>
    Task Create(CreateSensitiveWordViewModel model);

    /// <summary>
    /// 根据主键更新指定敏感词
    /// </summary>
    /// <param name="model">新增模型</param>
    Task Update(UpdateSensitiveWordViewModel model);
}