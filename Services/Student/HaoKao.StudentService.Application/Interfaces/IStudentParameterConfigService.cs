using HaoKao.StudentService.Application.ViewModels;

namespace HaoKao.StudentService.Application.Interfaces;

public interface IStudentParameterConfigService : IAppWebApiService, IManager
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
    /// 创建学员参数设置
    /// </summary>
    /// <param name="model">新增模型</param>
    Task Create(CreateStudentParameterConfigViewModel model);

    /// <summary>
    /// 根据主键删除指定学员参数设置
    /// </summary>
    /// <param name="id">主键</param>
    Task Delete(Guid id);

    /// <summary>
    /// 根据主键更新指定学员参数设置
    /// </summary>
    /// <param name="model">新增模型</param>
    Task Update(UpdateStudentParameterConfigViewModel model);
}