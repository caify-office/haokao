using HaoKao.ProductService.Application.ViewModels.SupervisorClass;

namespace HaoKao.ProductService.Application.Services.Management;

public interface ISupervisorClassService : IAppWebApiService, IManager
{
     /// <summary>
     /// 根据主键获取指定
     /// </summary>
     /// <param name="id">主键</param>
     Task<BrowseSupervisorClassViewModel> Get(Guid id);

     /// <summary>
     /// 根据查询获取列表，用于分页
     /// </summary>
     /// <param name="queryViewModel">查询对象</param>
     Task<SupervisorClassQueryViewModel> Get(SupervisorClassQueryViewModel queryViewModel);

     /// <summary>
     /// 创建班级督学
     /// </summary>
     /// <param name="model">新增模型</param>
     Task Create(CreateSupervisorClassViewModel model);

     /// <summary>
     /// 根据主键删除指定班级督学
     /// </summary>
     /// <param name="id">主键</param>
     Task Delete(Guid id);

     /// <summary>
     /// 根据主键更新指定班级督学
     /// </summary>
     /// <param name="model">新增模型</param>
     Task Update(UpdateSupervisorClassViewModel model);
}