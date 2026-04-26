using HaoKao.StudentService.Application.ViewModels;

namespace HaoKao.StudentService.Application.Interfaces;

public interface IStudentAllocationService : IAppWebApiService, IManager
{
    /// <summary>
    /// 分页查询学员分配
    /// </summary>
    /// <param name="input">查询条件</param>
    /// <returns></returns>
    Task<QueryStudentAllocationViewModel> Get(QueryStudentAllocationViewModel input);

    /// <summary>
    /// 批量修改分配
    /// </summary>
    /// <param name="input">输入</param>
    /// <returns></returns>
    Task UpdateAllocateTo(UpdateAllocateToViewModel input);

    /// <summary>
    /// 备注
    /// </summary>
    /// <param name="input">输入</param>
    /// <returns></returns>
    Task UpdateRemark(UpdateRemarkViewModel input);
}