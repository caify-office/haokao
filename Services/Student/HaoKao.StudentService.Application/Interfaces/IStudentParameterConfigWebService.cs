using HaoKao.StudentService.Application.ViewModels;

namespace HaoKao.StudentService.Application.Interfaces;

public interface IStudentParameterConfigWebService : IAppWebApiService, IManager
{
    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    Task<BrowseStudentParameterConfigViewModel> Get(Guid id);

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="model">查询对象</param>
    Task<QueryStudentParameterConfigViewModel> Get(QueryStudentParameterConfigViewModel model);

    /// <summary>
    /// 保存学员参数设置
    /// </summary>
    /// <param name="model">新增模型</param>
    Task Save(CreateStudentParameterConfigViewModel model);


    /// <summary>
    /// 条件查询(隔日生效)
    /// </summary>
    /// <param name="model">查询对象</param>
    Task<BrowseStudentParameterConfigViewModel> GetEffectiveNextDay(StudentParameterConfigQueryEffectiveNextDayViewModel model);


    /// <summary>
    /// 保存学员参数设置(隔日生效)
    /// </summary>
    /// <param name="model">新增模型</param>
    Task SaveEffectiveNextDay(CreateStudentParameterConfigViewModel model);
}