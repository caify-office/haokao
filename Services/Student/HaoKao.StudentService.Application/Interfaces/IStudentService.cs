using HaoKao.StudentService.Application.ViewModels;

namespace HaoKao.StudentService.Application.Interfaces;

public interface IStudentService : IAppWebApiService, IManager
{
    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="model">查询对象</param>
    Task<QueryStudentViewModel> Get(QueryStudentViewModel model);
}