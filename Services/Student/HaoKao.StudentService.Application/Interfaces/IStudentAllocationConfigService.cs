using HaoKao.StudentService.Application.ViewModels;

namespace HaoKao.StudentService.Application.Interfaces;

public interface IStudentAllocationConfigService : IAppWebApiService, IManager
{
    /// <summary>
    /// 获取当前租户下的学员分配配置
    /// </summary>
    /// <returns></returns>
    Task<BrowseStudentAllocationConfigViewModel> Get();

    /// <summary>
    /// 保存学员分配配置
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task Create(SaveStudentAllocationConfigViewModel model);
}