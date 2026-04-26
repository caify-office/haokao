using HaoKao.StudentService.Application.ViewModels;

namespace HaoKao.StudentService.Application.Interfaces;

public interface IStudentWeChatService : IAppWebApiService, IManager
{
    /// <summary>
    /// 获取当前租户下的学员信息
    /// </summary>
    Task<BrowseStudentViewModel> Get();
}