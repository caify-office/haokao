using HaoKao.ProductService.Application.ViewModels.StudentPermission;

namespace HaoKao.ProductService.Application.Services.Management;

public interface IStudentPermissionService : IAppWebApiService, IManager
{
    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    Task<BrowseStudentPermissionViewModel> Get(Guid id);

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    Task<StudentPermissionQueryViewModel> Get(StudentPermissionQueryViewModel queryViewModel);

    /// <summary>
    /// 创建学员权限表
    /// </summary>
    /// <param name="model">新增模型</param>
    Task Create(CreateStudentPermissionViewModel model);

    /// <summary>
    /// 根据主键删除指定学员权限表
    /// </summary>
    /// <param name="id">主键</param>
    Task Delete(Guid id);

    /// <summary>
    /// 根据主键更新指定学员权限表
    /// </summary>
    /// <param name="id">主键</param>
    /// <param name="model">新增模型</param>
    Task ChangeState(Guid id, UpdateStudentPermissionStateViewModel model);

    Task ChangeExpiryTime(Guid id, UpdateStudentPermissionExpiryTimeViewModel model);
}