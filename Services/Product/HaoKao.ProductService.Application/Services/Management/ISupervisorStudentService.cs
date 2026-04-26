using HaoKao.ProductService.Application.ViewModels.SupervisorStudent;

namespace HaoKao.ProductService.Application.Services.Management;

public interface ISupervisorStudentService : IAppWebApiService, IManager
{


     /// <summary>
     /// 根据查询获取列表，用于分页
     /// </summary>
     /// <param name="queryViewModel">查询对象</param>
     Task<SupervisorStudentQueryViewModel> Get(SupervisorStudentQueryViewModel queryViewModel);

     /// <summary>
     /// 创建督学学员
     /// </summary>
     /// <param name="model">新增模型</param>
     Task Create(CreateSupervisorStudentViewModel model);

     /// <summary>
     /// 根据主键删除指定督学学员
     /// </summary>
     /// <param name="id">主键</param>
     Task Delete(Guid id);

     /// <summary>
     /// 根据主键更新指定督学学员
     /// </summary>
     /// <param name="model">新增模型</param>
     Task Update(UpdateSupervisorStudentViewModel model);


    /// <summary>
    /// 更新督学学员统计数据
    /// </summary>
    Task UpdateStatisticsData(Guid supervisorClassId);
}